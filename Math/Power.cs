using CAS;

public class Power : MathObject {
    public MathObject Base {get; private set;}
    public MathObject exponent {get; private set;}
    public Power(MathObject obj1, MathObject obj2) {
        Base = obj1;
        exponent = obj2;
    }
    
    public MathObject Evaluate(Dictionary<string, double> definedVariables) {
        //calculate all terms
        return new Power(Base.Calculate(definedVariables),exponent.Calculate(definedVariables));
    }

    public MathObject Simplify() {
        //combine constants
        if(Base is Constant && exponent is Constant) return new Constant(Math.Pow(((Constant)Base).value,((Constant)exponent).value));
        
        return this;
    }

    public bool Equals(MathObject obj) =>
        obj is Power &&                              //same type
        ((Power)obj).Base.Equals(this.Base) &&       //same terms
        ((Power)obj).exponent.Equals(this.exponent); //same terms
        

    public bool EquivalentTo(MathObject obj) =>
        obj.Evaluate().Equals(this.Evaluate(new()));

    public string AsString() => Base.AsString()+"^"+exponent.AsString();
}