namespace CAS;

public class Power : MathObject {
    public MathObject Base {get; private set;}
    public MathObject exponent {get; private set;}
    public Power(MathObject Base, MathObject exponent) {
        this.Base = Base;
        this.exponent = exponent;
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
        var bas = Base.Simplify();
        var exp = exponent.Simplify();

        if (exp is Constant) {
            var num = exp.AsValue();
            //a^0 = 1
            if(num==0) return new Constant(1);

            //a^1 = a
            if(num==1) return bas;

            //combine constants
            if(bas is Constant) return new Constant(Math.Pow(bas.AsValue(),num));
        }
        return new Power(bas,exp);
    }

    public bool Equals(MathObject obj) =>
        obj is Power &&                              //same type
        ((Power)obj).Base.Equals(this.Base) &&       //same terms
        ((Power)obj).exponent.Equals(this.exponent); //same terms
        
    public bool ContainsAny(MathObject obj) => 
        obj.Equals(this) || 
        Base.Equals(obj) || exponent.Equals(obj) ||
        Base.ContainsAny(obj) || exponent.ContainsAny(obj);

    
    public string AsString() => 
        (Base.Precedence()!=0 && Base.AbsPrecedence()<Math.Abs(this.Precedence())? "("+Base.AsString()+")":Base.AsString()) + "^"+
        (exponent.Precedence()!=0 && exponent.AbsPrecedence()<Math.Abs(this.Precedence())? "("+exponent.AsString()+")":exponent.AsString());

    public int Precedence() => Operator.Precedence('^');
}