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
    a*a = a^2
    a*a^b = a^(b+1)
    a^b*a^c = a^(b+c)
    0*a = 0
    1*a = a
    */

    public MathObject Simplify() {
        //simplify terms
        terms = terms.Select(term => term.Simplify()).ToList();
        
        //combine constants
        double value = 1;
        var temp = terms.Where(term => term is Constant).ToList();
        foreach(Constant constant in temp) {
            terms.Remove(constant);
            value *= constant.value;            
        }
        if (value!=1) terms.Add(new Constant(value));

        if(terms.Count==1) return terms[0];
        return this;
    }

    public bool Equals(MathObject obj) =>
        obj is Multiply &&                                //same type
        ((Multiply)obj).terms.SequenceEqual(this.terms);  //same terms

    public bool EquivalentTo(MathObject obj) =>
        obj.Evaluate().Equals(this.Evaluate(new()));

    public string AsString() => string.Join("*",terms.Select(term => term.Precedence()!=0 && term.AbsPrecedence()<Math.Abs(this.Precedence())? "("+term.AsString()+")":term.AsString()));
    public int Precedence() => Operator.Precedence('*');
}