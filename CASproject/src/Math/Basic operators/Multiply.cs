namespace CAS;

public class Multiply : MathObject {
    public List<MathObject> terms {get; private set;} = new();
    public Multiply(MathObject obj1, MathObject obj2) : this([obj1,obj2]) {}
    public Multiply(IEnumerable<MathObject> terms) {
        //combine all multiply terms under this multiply
        foreach(var term in terms) {
            if(term is Multiply) this.terms.AddRange(((Multiply)term).terms);
            else this.terms.Add(term);
        }
    }

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        //calculate all terms
        return new Multiply(terms.Select(term => term.Evaluate(definedObjects)));
    }

    /*
    Simplifications:
    */
    public MathObject Simplify() {
        //simplify terms
        var terms = this.terms.Select(term => term.Simplify()).ToList();
        
        //combine divide
        if(terms.Any(n => n is Divide)) {
            List<MathObject> denoms = new();
            foreach(Divide div in terms.Where(n => n is Divide).ToList()) {
                terms.Remove(div);
                terms.Add(div.numerator);
                denoms.Add(div.denominator);
            }
            var divN = terms.Count<=1? terms[0]:new Multiply(terms);
            var divD = denoms.Count<=1? denoms[0]:new Multiply(denoms);
            return new Divide(divN,divD).Simplify();
        }

        MathObject obj = this;
        int index = 0;
        for(int i=0;i<terms.Count;i++) {
            var term = terms[i];
            if(term is Constant) continue;

            if (term is Power pow) {
                //a * a^n = a^(n+1)
                if (MathObject.FindAndRemoveOtherTerm(t => t.Equals(pow.Base),terms,ref i, ref obj, ref index)) {
                    terms[i] = new Power(pow.Base,new Add(pow.exponent,new Constant(1d)).Simplify());
                    i=-1; continue;
                }
                
                //a^b * a^c = a^(b+c)
                if (MathObject.FindAndRemoveOtherTerm(term => (term as Power)?.Base.Equals(pow.Base)??false, terms,ref i,ref obj, ref index)) {
                    terms[i] = new Power(pow.Base,new Add(pow.exponent,obj.As<Power>().exponent).Simplify());
                    i=-1; continue;
                }
            }

            //a*a = a^2
            if (MathObject.FindAndRemoveOtherTerm(t => t.Equals(term),terms,ref i, ref obj, ref index)) {
                terms[i] = new Power(term,new Constant(2d));
                i=-1; continue;
            }
        }

        //combine constants
        Constant value = 1;
        var temp = terms.Where(term => term is Constant).ToList();
        foreach(Constant constant in temp) {
            terms.Remove(constant);
            value *= constant;            
        }
        if(value.IsZero) return new Constant(0d);                               //0*a = 0
        if(!value.IsOne || terms.Count==0) terms.Insert(0,new Constant(value));	//1*a = a

        if(terms.Count==1) return terms[0];
        if(terms.Count==0) return new Constant(0d);
        return new Multiply(terms);
    }

    public MathObject Differentiate(string variable) {
        var constants = this.terms.Where(n => n is Constant).ToList();
        var terms = this.terms.Where(n => n is not Constant).ToList();
        if(terms.Count==1) {
            if(constants.Count>0) return new Multiply(constants.Append(terms[0].Differentiate(variable)));
            return terms[0].Differentiate(variable);
        }
        
        if(terms.Count==0) return (Constant)0;
        MathObject current = terms[0];
        for(int i=1;i<terms.Count;i++) {
            var next = terms[i];
            //(fg)' = f'g + fg'
            current = new Add(new Multiply(current.Differentiate(variable),next),new Multiply(current,next.Differentiate(variable)));
        }
        return new Multiply(constants.Append(current));
    }

    public bool ContainsAny(MathObject obj) => 
        obj.Equals(this) || 
        terms.Any(term => term.ContainsAny(obj) || term.Equals(obj)); 
    
    
    public bool Equals(MathObject obj) {
        //same type
        if(!(obj is Multiply m)) return false;    
        
        //same terms
        var objTerms = m.terms.ToList();
        if(objTerms.Count!=terms.Count) return false;
        foreach(var term in terms) {
            if(objTerms.FindOtherTerm(n => n.Equals(term),out int index)) objTerms.RemoveAt(index);
            else return false;
        }
        return true;
    }
    
    public string AsString() => string.Join("*",terms.Select((term,i) => 
        term.Precedence()!=0 && term.AbsPrecedence()<Math.Abs(this.Precedence())?    "("+term.AsString()+")":   //parentheses
        term.AsString()                                                                                         //default
    )).Replace("-1*","-");
    
    public MathObject WithoutFirstTerm() {
        return terms.Count>2? 
            new Multiply(terms.Skip(1)): 
            terms[1];
    }

    public int Precedence() => Operator.Precedence('*');
} 