namespace CAS;

public class Add : MathObject {
    public List<MathObject> terms {get; private set;} = new();
    public Add(MathObject obj1, MathObject obj2) : this([obj1,obj2]) {}
    public Add(IEnumerable<MathObject> terms) {
        //combine all add terms under this add
        foreach(var term in terms) {
            if(term is Add) this.terms.AddRange(((Add)term).terms);
            else this.terms.Add(term);
        }
    }

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        //calculate all terms
        return new Add(terms.Select(term => term.Evaluate(definedObjects)));
    }

    /*
    Simplifications:
    a + n*a -> (n+1)*a), n is R
    a + a -> 2*a
    n*a + m*a -> (n+m)*a,  n is R, m is R
    a/c + b/c = (a+b)/c
    */
    public MathObject Simplify() {
        //simplify terms
        terms = terms.Select(term => term.Simplify()).ToList();

        //combine constants
        double value = 0;
        var temp = terms.Where(term => term is Constant).ToList();
        foreach(Constant constant in temp) {
            terms.Remove(constant);
            value += constant.value;            
        }
        if(value!=0) terms.Add(new Constant(value));

        if(terms.Count==1) return terms[0];
        if(terms.Count==0) return new Constant(0);
        return this;
    }

    public bool Contains(MathObject obj) => 
        obj.Equals(this) || 
        terms.Any(term => term.Equals(obj) || term.Contains(obj)); 
    
    public bool Equals(MathObject obj) {
        //same type
        if(!(obj is Add)) return false;    
        
        //same terms
        var objTerms = ((Add)obj).terms.ToHashSet();
        return terms.All(term => objTerms.Any(n => n.Equals(term))); 
    }

    public bool EquivalentTo(MathObject obj) => throw new NotImplementedException();

    public string AsString() => string.Join("+",terms.Select(term => term.Precedence()!=0 && term.AbsPrecedence()<Math.Abs(this.Precedence())? "("+term.AsString()+")":term.AsString())).Replace("+-","-");
    public int Precedence() => Operator.Precedence('+');

    public static Multiply Negate(MathObject obj) => new Multiply(new Constant(-1),obj);
}