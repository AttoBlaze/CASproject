namespace CAS;
using Application;

/// <summary>
/// Represents a wrapper for a math object; this is effectively a stored transformation on a mathobject. 
/// </summary>
public abstract class MathWrapper : MathObject {
    public virtual MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => transformation(expression).Evaluate(definedObjects);
    public virtual MathObject Simplify() => transformation(expression).Simplify();
    public virtual bool Equals(MathObject obj) => 
        obj is MathWrapper wrapper &&
        wrapper.transformation==this.transformation &&
        wrapper.name==this.name &&
        wrapper.expression.Equals(this.expression);
    public virtual bool ContainsAny(MathObject obj) => obj.Equals(this) || transformation(expression).ContainsAny(obj);
    public virtual string AsString() => name+"("+expression.AsString()+")";
    public MathObject expression {get; protected set;} = new Constant(0);
    public string name {get; protected set;} = "";
    public Func<MathObject,MathObject> transformation {get; protected set;} = obj => obj;
}   
public class InformalMathWrapper : MathWrapper { 
    public InformalMathWrapper(string name, Func<MathObject,MathObject> transformation,MathObject expression) {
        this.expression = expression;
        this.name = name;
        this.transformation = transformation;
    }
}
public sealed class SimplifyExpression : MathWrapper {
    public override MathObject Simplify() => expression.Simplify();
    public SimplifyExpression(MathObject expression) {
        this.expression = expression;
        this.name = "Simplify";
        this.transformation = obj => obj.Simplify();
    }
}
public sealed class EvaluateExpression : MathWrapper {
    public override MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => expression.Evaluate(Program.definedObjects);
    public EvaluateExpression(MathObject expression) {
        this.expression = expression;
        this.name = "Evaluate";
        this.transformation = obj => obj.Evaluate(Program.definedObjects);
    }
}
public sealed class CalculateExpression : MathWrapper {
    public CalculateExpression(MathObject expression) {
        this.expression = expression;
        this.name = "Calculate";
        this.transformation = obj => obj.Calculate(Program.definedObjects);
    }
}