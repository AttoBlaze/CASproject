
namespace CAS;

public class Ln : MathObject {
    public readonly MathObject expression;
    public Ln(MathObject expression) {
        this.expression = expression;
    }

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) =>
        new Ln(expression.Evaluate(definedObjects));

    public MathObject Simplify() {
        var expr = expression.Simplify();
        
        //constant
        if(expr is Constant num) return new Constant(Math.Log(num.value));
        
        //e
        if(expr is Variable e && e.name=="e") return new Constant(1);

        //e^x
        if(((expr as Power)?.Base as Variable)?.name=="e") return expr.As<Power>().exponent;
        
        return new Ln(expr);
    }

    public MathObject Differentiate(string variable) =>
        new Divide(new Constant(1),expression);

    public bool Equals(MathObject obj) =>
        obj is Ln ln &&
        ln.expression.Equals(this.expression);

    public bool ContainsAny(MathObject obj) => obj.Equals(this) || expression.ContainsAny(obj);

    public string AsString() => "ln("+expression.AsString()+")";
}