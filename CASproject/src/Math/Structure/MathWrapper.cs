namespace CAS;
using Application;

/// <summary>
/// Represents a wrapper for a math object; this is effectively a stored transformation on a mathobject (such as simplify).
/// </summary>
public abstract class MathWrapper : MathObject {
    public virtual MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => transformation(expression).Evaluate(definedObjects);
    public virtual MathObject Simplify(SimplificationSettings settings) => transformation(expression).Simplify(settings);
    public virtual MathObject Differentiate(string variable, CalculusSettings settings) => transformation(expression).Differentiate(variable,settings);
    public virtual bool Equals(MathObject obj) => 
        obj is MathWrapper wrapper &&
        wrapper.transformation==this.transformation &&
        wrapper.name==this.name &&
        wrapper.expression.Equals(this.expression);
    public virtual bool ContainsAny(MathObject obj) => obj.Equals(this) || obj.Equals(expression) || expression.ContainsAny(obj) || transformation(expression).ContainsAny(obj);
    public virtual string AsString() => name+"("+expression.AsString()+")";
    public MathObject expression {get; protected set;} = new Constant(0d);
    public string name {get; protected set;} = "";
    public Func<MathObject,MathObject> transformation {get; protected set;} = obj => obj;
}   
public class InformalMathWrapper : MathWrapper { 
    public InformalMathWrapper(string name, Func<MathObject,MathObject> transformation, MathObject expression) {
        this.expression = expression;
        this.name = name;
        this.transformation = transformation;
    }
}

public class SimplifyExpression : MathWrapper {
    public override MathObject Simplify(SimplificationSettings settings) => expression.Simplify();
    public SimplifyExpression(MathObject expression) {
        this.expression = expression;
        this.name = "Simplify";
        this.transformation = obj => obj.Simplify();
    }
}
public class EvaluateExpression : MathWrapper {
    public override MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => expression.Evaluate(Program.definedObjects);
    public EvaluateExpression(MathObject expression) {
        this.expression = expression;
        this.name = "Evaluate";
        this.transformation = obj => obj.Evaluate(Program.definedObjects);
    }
}
public class CalculateExpression : MathWrapper {
    public CalculateExpression(MathObject expression) {
        this.expression = expression;
        this.name = "Calculate";
        this.transformation = obj => obj.Calculate();
    }
}
public class DerivativeExpression : MathWrapper {
    public override MathObject Differentiate(string variable, CalculusSettings settings) => expression.Differentiate(variable,settings);
    public override string AsString() => name+"("+expression.AsString()+";"+variable+")";
    public readonly string variable;
    public DerivativeExpression(MathObject expression, string variable) {
        this.expression = expression;
        this.name = "Derivative";
        this.variable = variable;
        this.transformation = obj => obj.Differentiate(variable);
    }
}

public class DiffExpression : MathWrapper {
    public override string AsString() => name+"("+expression.AsString()+";"+variable+")";
    public readonly string variable;
    public DiffExpression(MathObject expression, string variable) {
        this.expression = expression;
        this.name = "Diff";
        this.variable = variable;
        this.transformation = obj => obj.Diff(variable);
    }
}