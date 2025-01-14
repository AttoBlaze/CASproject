namespace CAS;
using Application;

public sealed class SimplifyExpression : MathObject {
    public MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => expression.Simplify().Evaluate(definedObjects);
    public MathObject Simplify() => expression.Simplify();
    public bool Equals(MathObject obj) => expression.Simplify().Equals(obj);
    public string AsString() => "Simplify("+expression.AsString()+")";
    private readonly MathObject expression;
    public SimplifyExpression(MathObject expression) {
        this.expression = expression;
    }
}
public sealed class EvaluateExpression : MathObject {
    public MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => expression.Evaluate(Program.definedObjects);
    public MathObject Simplify() => expression.Evaluate(Program.definedObjects).Simplify();
    public bool Equals(MathObject obj) => expression.Evaluate(Program.definedObjects).Equals(obj);
    public string AsString() => "Evaluate("+expression.AsString()+")";
    private readonly MathObject expression;
    public EvaluateExpression(MathObject expression) {
        this.expression = expression;
    }
}
public sealed class CalculateExpression : MathObject {
    public MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => expression.Calculate();
    public MathObject Simplify() => expression.Calculate();
    public bool Equals(MathObject obj) => expression.Calculate().Equals(obj);
    public string AsString() => "Calculate("+expression.Calculate()+")";
    private readonly MathObject expression;
    public CalculateExpression(MathObject expression) {
        this.expression = expression;
    }
}