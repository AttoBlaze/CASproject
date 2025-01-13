using CAS;

public class Divide : MathObject {
    public MathObject numerator {get; private set;}
    public MathObject denominator {get; private set;}
    public Divide(MathObject numerator, MathObject denominator) {
        this.numerator = numerator;
        this.denominator = denominator;
    }
    
    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        //calculate all terms
        return new Divide(numerator.Evaluate(definedObjects),denominator.Evaluate(definedObjects));
    }

    /*
    Simplifications:
    a/(a^b) = a^(1-b)
    (a^b)/a = a^(b-1)
    (a^b)/(a^c) = a^(b-c)
    a/(b/c) = (a*c)/b
    */
    public MathObject Simplify() {
        var num = numerator.Simplify();
        var denom = denominator.Simplify();

        //a/a = 1
        if(num.Equals(denom)) return new Constant(1);

        //combine constants
        if(num is Constant && denom is Constant) return new Constant(((Constant)num).value/((Constant)denom).value);
        return this;
    }

    public bool Contains(MathObject obj) => 
        obj.Equals(this) || 
        numerator.Equals(obj) || denominator.Equals(obj) ||
        numerator.Contains(obj) || denominator.Contains(obj);

    public bool Equals(MathObject obj) =>
        obj is Divide &&                                    //same type
        ((Divide)obj).numerator.Equals(this.numerator) &&   //same terms
        ((Divide)obj).denominator.Equals(this.denominator); //same terms
        

    public bool EquivalentTo(MathObject obj) => throw new NotImplementedException();

    public string AsString() => 
        (numerator.Precedence()!=0 && numerator.AbsPrecedence()<Math.Abs(this.Precedence())? "("+numerator.AsString()+")":numerator.AsString()) + "/"+
        (denominator.Precedence()!=0 && denominator.AbsPrecedence()<Math.Abs(this.Precedence())? "("+denominator.AsString()+")":denominator.AsString());
    public int Precedence() => Operator.Precedence('/');
}