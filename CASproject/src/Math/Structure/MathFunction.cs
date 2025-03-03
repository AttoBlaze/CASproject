namespace CAS;

/// <summary>
/// A wrapper for streamlining simple formal functions.
/// </summary>
public abstract class MathFunction : MathObject {
    public string name {get; protected set;} = "";
    public MathObject expression {get; protected set;} = (Constant)0;
    protected abstract MathObject Create(MathObject obj);
    public virtual MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => Create(expression.Evaluate(definedObjects));
    public virtual MathObject Simplify(SimplificationSettings settings) => Create(expression.Simplify(settings));
    public virtual MathObject Differentiate(string variable, CalculusSettings settings) => this.Differentiate(variable,settings);
    public virtual string AsString() => name+"("+expression.AsString()+")";
    public virtual bool Equals(MathObject obj) =>
        obj.GetType()==this.GetType() &&
        obj.As<MathFunction>().expression.Equals(this.expression);
    public virtual bool ContainsAny(MathObject obj) =>
        obj.Equals(this) || expression.Equals(obj) || expression.ContainsAny(obj);
}