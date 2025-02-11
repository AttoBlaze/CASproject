namespace CAS;

public class Add : MathObject {
    public List<MathObject> terms {get; private set;} = new();
    public Add(MathObject obj1, MathObject obj2) : this([obj1,obj2]) {}
    public Add(IEnumerable<MathObject> terms) {
        //combine all add terms under this add
        foreach(var term in terms) { 
            if(term is Add) this.terms.AddRange(((Add)term).terms);
            else if(term is not Constant num || num.value!=0) this.terms.Add(term);
        }
    }

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        //calculate all terms
        return new Add(terms.Select(term => term.Evaluate(definedObjects)));
    }

    /*
    Simplifications:
    contraction: a*b + c*b = (a+c)*b
    */
    public MathObject Simplify() {
        //simplify terms
        var terms = this.terms.Select(term => term.Simplify()).ToList();
        
        MathObject obj = this;
        int index = 0;
        for(int i=0;i<terms.Count;i++) {
            var term = terms[i];
            if(term is Constant) continue;

            if ((term as Multiply)?.terms[0] is Constant) {
                //a + n*a = (n+1)*a
                var num = term.As<Multiply>().terms[0].AsValue(); 
                MathObject mult = term.As<Multiply>().WithoutFirstTerm();
                if(MathObject.FindAndRemoveOtherTerm(term => term.Equals(mult),terms,ref i,ref obj ,ref index)) {
                    if(num+1==0) terms.RemoveAt(i);                             //a+(-1*a) = 0
                    else terms[i] = new Multiply(new Constant(num+1),mult);
                    i=-1; continue;
                }

                //b*a + c*a = (b+c)*a
                if(MathObject.FindAndRemoveOtherTerm(t => 
                    t is Multiply && t.As<Multiply>().terms[0] is Constant && t.As<Multiply>().WithoutFirstTerm().Equals(mult)
                    ,terms,ref i,ref obj ,ref index)) 
                {   
                    var num2 = obj.As<Multiply>().terms[0].AsValue();
                    if(num+num2==0) terms.RemoveAt(i);                             //n*a+(-n*a) = 0
                    else if (num+num2==1) terms[i] = mult;						   //1*a = a
					else terms[i] = new Multiply(new Constant(num+num2),mult);
                    i=-1; continue;
                }
            }

            //a/b + c/b = (a+c)/b
            if(term is Divide div1) {
                if(MathObject.FindAndRemoveOtherTerm(n => n is Divide div2 && div2.denominator.Equals(div1.denominator),terms,ref i, ref obj, ref index)) {
                    terms[i] = new Divide(new Add(div1.numerator,obj.As<Divide>().numerator),div1.denominator).Simplify();
                    i=-1; continue;
                }
            }

            //a+a = 2*a
            if(MathObject.FindAndRemoveOtherTerm(t => t.Equals(term),terms,ref i,ref obj,ref index)) {
                terms[i] = new Multiply(new Constant(2),term);
                i=-1; continue;
            }
        }

        //combine constants
        double value = 0;
        var temp = terms.Where(term => term is Constant).ToList();
        foreach(var constant in temp) {
            terms.Remove(constant);
            value += constant.AsValue();          
        }
        if(value!=0) terms.Add(new Constant(value));

        if(terms.Count==1) return terms[0];
        if(terms.Count==0) return new Constant(0);
        return new Add(terms);
    }

    public bool ContainsAny(MathObject obj) => 
        obj.Equals(this) || 
        terms.Any(term => term.Equals(obj) || term.ContainsAny(obj)); 
    
    public bool Equals(MathObject obj) {
        //same type
        if(!(obj is Add m)) return false;    

        //same terms
        var objTerms = m.terms.ToList();
        if(objTerms.Count!=terms.Count) return false;
        foreach(var term in terms) {
            if(objTerms.FindOtherTerm(n => n.Equals(term),out int index)) objTerms.RemoveAt(index);
            else return false;
        }
        return true;
    }

    public MathObject Differentiate(string variable) {
        //(f+g)' = f' + g'
        var terms = this.terms.Where(n => n is not Constant).Select(n => n.Differentiate(variable)).ToList();
        if(terms.Count==1) return terms[0];
        if(terms.Count==0) return new Constant(0);
        return new Add(terms);
    }
    

    public string AsString() => string.Join("+",terms.Select(term => term.Precedence()!=0 && term.AbsPrecedence()<Math.Abs(this.Precedence())? "("+term.AsString()+")":term.AsString())).Replace("+-","-");
    public int Precedence() => Operator.Precedence('+');

    public static MathObject Negate(MathObject obj) => obj is Constant o? new Constant(-(o.value)): new Multiply(new Constant(-1),obj);
}