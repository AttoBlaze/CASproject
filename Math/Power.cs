namespace CAS;

public class Power : MathObject {
    public MathObject Base {get; private set;}
    public MathObject exponent {get; private set;}
    public Power(MathObject obj1, MathObject obj2) {
        Base = obj1;
        exponent = obj2;
    }
    
    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        //calculate all terms
        return new Power(Base.Calculate(definedObjects),exponent.Calculate(definedObjects));
    }

    /*
    Simplifications:
    a^b = c,  a is R, b is R, c is R
    (a^b)^c) = a^(b*c)
    a^0 = 1
    a^1 = a
    */

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