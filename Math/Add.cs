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
        return this;
    }

    public  bool Equals(MathObject obj) =>
        obj is Add &&                                //same type
        ((Add)obj).terms.SequenceEqual(this.terms);  //same terms

    public bool EquivalentTo(MathObject obj) =>
        obj.Evaluate().Equals(this.Evaluate(new()));

    public string AsString() => string.Join("+",terms.Select(term => term.AsString())).Replace("+-","-");

    public static Multiply Negate(MathObject obj) => new Multiply(new Constant(-1),obj);
}