
namespace CAS;

public class Ln : MathObject {
    public MathObject expression;
    public Ln(MathObject expression) {
        this.expression = expression;
    }

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) =>
        new Ln(expression.Evaluate(definedObjects));

    public MathObject Simplify() {
        var expr = expression.Simplify();
        if(expr is Constant) return new Constant(Math.Log(expr.AsValue()));
        return new Ln(expr);
    }

    public bool Equals(MathObject obj) =>
        obj is Ln &&
        ((Ln)obj).expression.Equals(this.expression);

    public bool EquivalentTo(MathObject obj) => throw new NotImplementedException();

    public bool Contains(MathObject obj) => obj.Equals(this) || expression.Contains(obj);

    public string AsString() => "ln("+expression.AsString()+")";
}