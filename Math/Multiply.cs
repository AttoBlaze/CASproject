using CAS;

public class Multiply : MathObject {
    public List<MathObject> terms {get; private set;}
    public Multiply(MathObject obj1, MathObject obj2) : this([obj1,obj2]) {}
    public Multiply(IEnumerable<MathObject> terms) {
        this.terms = terms.Where(n => !(n is Multiply)).ToList();
        
        //combine all add terms
        foreach(Multiply term in terms.Where(n => n is Multiply)) {
            this.terms.AddRange(term.terms);
        }
    }

    public MathObject Evaluate(Dictionary<string, double> definedVariables) {
        //calculate all terms
        return new Multiply(terms.Select(term => term.Calculate(definedVariables)));
    }

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
        terms.Add(new Constant(value));

        if(terms.Count==1) return terms[0];
        return this;
    }

    public bool Equals(MathObject obj) =>
        obj is Multiply &&                                //same type
        ((Multiply)obj).terms.SequenceEqual(this.terms);  //same terms

    public bool EquivalentTo(MathObject obj) =>
        obj.Evaluate().Equals(this.Evaluate(new()));

    public string AsString() => string.Join("*",terms.Select(term => term.AsString()));
}