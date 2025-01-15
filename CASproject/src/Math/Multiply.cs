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

        MathObject obj = this;
        int index = 0;
        for(int i=0;i<terms.Count;i++) {
            var term = terms[i];
            if(term is Constant) continue;

            if (term is Power pow) {
                //a * a^n = a^(n+1)
                if (MathObject.FindAndRemoveOtherTerm(t => t.Equals(pow.Base),terms,ref i, ref obj, ref index)) {
                    terms[i] = new Power(pow.Base,new Add(pow.exponent,new Constant(1)).Simplify());
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
                terms[i] = new Power(term,new Constant(2));
                i=-1; continue;
            }
        }

        //combine constants
        double value = 1;
        var temp = terms.Where(term => term is Constant).ToList();
        foreach(Constant constant in temp) {
            terms.Remove(constant);
            value *= constant.value;            
        }
        if(value==0) return new Constant(0);                //0*a = 0
        if (value!=1) terms.Insert(0,new Constant(value));  //1*a = a

        if(terms.Count==1) return terms[0];
        if(terms.Count==0) return new Constant(1);
        return new Multiply(terms);
    }

    public bool ContainsAny(MathObject obj) => 
        obj.Equals(this) || 
        terms.Any(term => term.ContainsAny(obj) || term.Equals(obj)); 
    
    
    public bool Equals(MathObject obj) {
        //same type
        if(!(obj is Multiply)) return false;    
        
        //same terms
        var objTerms = ((Multiply)obj).terms;
        return terms.All(term => objTerms.Any(n => n.Equals(term))); 
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