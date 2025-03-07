using Application;

namespace CAS;

public class Add : MathObject {
    public List<MathObject> terms {get; private set;} = new();
    public Add(MathObject obj1, MathObject obj2) : this([obj1,obj2]) {}
    public Add(IEnumerable<MathObject> terms) {
        foreach(var term in terms) { 
        	//combine all add terms under this add
            if(term is Add) this.terms.AddRange(((Add)term).terms);
            
			//dont add terms that are 0
			else if(term is not Constant num || !num.IsZero) this.terms.Add(term);
        }
    }

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        //evaluate all terms
        return new Add(terms.Select(term => term.Evaluate(definedObjects)));
    }

    /*
    Simplifications:
    contraction: a*b + c*b = (a+c)*b
    */
    public MathObject Simplify(SimplificationSettings settings) {
        //simplify terms
        var terms = this.terms.Select(term => term.Simplify(settings)).ToList();

		//go through terms to look for simplifications
        MathObject obj = this;
        int index = 0;
        for(int i=0;i<terms.Count;i++) {
            var term = terms[i];
            if(term is Constant c) continue;

            if ((term as Multiply)?.terms[0] is Constant) {
                var num = term.As<Multiply>().terms[0].As<Constant>();
                MathObject mult = term.As<Multiply>().WithoutFirstTerm();
                
				//a + n*a = (n+1)*a
                if(MathObject.FindAndRemoveOtherTerm(
					term => term.Equals(mult),terms,ref i,ref obj ,ref index)) {
                    var val = settings.calculator.add(num,1);
					if(val.IsZero) terms.RemoveAt(i);						//a+(-1*a) = 0
					else terms[i] = new Multiply(val,mult);
                    i=-1; continue;
                }

                //b*a + c*a = (b+c)*a
                if(MathObject.FindAndRemoveOtherTerm(
					term => term is Multiply && term.As<Multiply>().terms[0] is Constant && term.As<Multiply>().WithoutFirstTerm().Equals(mult)
                    ,terms,ref i,ref obj ,ref index)) 
                {   
					var val = settings.calculator.add(obj.As<Multiply>().terms[0].As<Constant>(),num);
					if(val.IsZero) terms.RemoveAt(i);                           	//n*a+(-n*a) = 0
                    else if (val.IsOne) terms[i] = mult;						    //1*a = a
					else terms[i] = new Multiply(val,mult);
                    i=-1; continue;
                }
            }

            //a/b + c/b = (a+c)/b
            if(term is Divide div1) {
                if(MathObject.FindAndRemoveOtherTerm(n => n is Divide div2 && div2.denominator.Equals(div1.denominator),terms,ref i, ref obj, ref index)) {
                    terms[i] = new Divide(
						new Add(div1.numerator, obj.As<Divide>().numerator),
						div1.denominator)
					.Simplify(settings);
                    i=-1; continue;
                }
            }

            //a+a = 2*a
            if(MathObject.FindAndRemoveOtherTerm(t => t.Equals(term),terms,ref i,ref obj,ref index)) {
                terms[i] = new Multiply(new Constant(2d),term);
                i=-1; continue;
            }
        }

        //combine constants
        if(settings.calculateConstants) {
			Constant value = 0;
			var temp = terms.Where(term => term is Constant).ToList();
			foreach(var constant in temp) {
				terms.Remove(constant);
				value = settings.calculator.add(value,(Constant)constant);          
			}
			if(!value.IsZero) terms.Add(new Constant(value));
		}

        if(terms.Count==1) return terms[0];
        if(terms.Count==0) return new Constant(0d);
        return new Add(terms);
    }

    public bool ContainsAny(MathObject obj) => 
        obj.Equals(this) || 
        terms.Any(term => term.Equals(obj) || term.ContainsAny(obj)); 
    
    public bool Equals(MathObject obj) =>
        obj is Add m && 												//same type
        MathObject.TermListsContainSameElements(m.terms,this.terms);	//same terms

    public MathObject Differentiate(string variable, CalculusSettings settings) {
        //(f+g)' = f' + g'
        var terms = this.terms.Where(n => n is not Constant).Select(n => n.Differentiate(variable,settings)).ToList();
        if(terms.Count==1) return terms[0];
        if(terms.Count==0) return new Constant(0d);
        return new Add(terms);
    }
    

    public string AsString() => string.Join("+",terms.Select(term => term.Precedence()!=0 && term.AbsPrecedence()<Math.Abs(this.Precedence())? "("+term.AsString()+")":term.AsString())).Replace("+-","-");
    public int Precedence() => Operator.Precedence('+');

    public static MathObject Negate(MathObject obj) => obj is Constant o? o.Negate(): new Multiply(new Constant(-1d),obj);
}