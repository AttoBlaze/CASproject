using CAS;

public class Divide : MathObject {
    public MathObject numerator {get; private set;}
    public MathObject denominator {get; private set;}
    public Divide(MathObject obj1, MathObject obj2) {
        numerator = obj1;
        denominator = obj2;
    }
    
    public MathObject Evaluate(Dictionary<string, double> definedVariables) {
        //calculate all terms
        return new Divide(numerator.Calculate(definedVariables),denominator.Calculate(definedVariables));
    }

    public MathObject Simplify() {
        //combine constants
        if(numerator is Constant && denominator is Constant) return new Constant(((Constant)numerator).value/((Constant)denominator).value);
        
        return this;
    }

    public bool Equals(MathObject obj) =>
        obj is Divide &&                                    //same type
        ((Divide)obj).numerator.Equals(this.numerator) &&   //same terms
        ((Divide)obj).denominator.Equals(this.denominator); //same terms
        

    public bool EquivalentTo(MathObject obj) =>
        obj.Evaluate().Equals(this.Evaluate(new()));

    public string AsString() => numerator.AsString()+"/"+denominator.AsString();
}