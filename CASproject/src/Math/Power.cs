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
        
    public bool Contains(MathObject obj) => 
        obj.Equals(this) || 
        Base.Equals(obj) || exponent.Equals(obj) ||
        Base.Contains(obj) || exponent.Contains(obj);

    public bool EquivalentTo(MathObject obj) => throw new NotImplementedException();
    public string AsString() => 
        (Base.Precedence()!=0 && Base.AbsPrecedence()<Math.Abs(this.Precedence())? "("+Base.AsString()+")":Base.AsString()) + "^"+
        (exponent.Precedence()!=0 && exponent.AbsPrecedence()<Math.Abs(this.Precedence())? "("+exponent.AsString()+")":exponent.AsString());

    public int Precedence() => Operator.Precedence('^');
}