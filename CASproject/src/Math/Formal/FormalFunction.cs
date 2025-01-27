namespace CAS;

using Application;

public sealed partial class FormalFunction {
    public static FormalFunction Get(string name) {
        if(Program.formalFunctions.TryGetValue(name, out FormalFunction? formalFunction)) return formalFunction;
        throw new Exception("Given formal function does not exist!");
    }

    public readonly Func<Stack<object>,MathObject> create;
    public readonly string name, description;
    public readonly string[] inputs;
    public FormalFunction(string name, string description, string[] inputs, Func<Stack<object>,MathObject> create) {
        this.name = name;
        this.description = description;
        this.inputs = inputs;
        this.create = create;
        Program.formalFunctions[name] = this;
    }
}

public abstract class MathFunction : MathObject {
    public string name {get; protected set;} = "";
    public MathObject expression {get; protected set;} = (Constant)0;
    protected abstract MathObject Create(MathObject obj);
    public virtual MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => Create(expression.Evaluate(definedObjects));
    public virtual MathObject Simplify() => Create(expression.Simplify());
    public virtual MathObject Differentiate(string variable) => this.Differentiate(variable);
    public virtual string AsString() => name+"("+expression.AsString()+")";
    public virtual bool Equals(MathObject obj) =>
        obj.GetType()==this.GetType() &&
        obj.As<MathFunction>().expression.Equals(this.expression);
    public virtual bool ContainsAny(MathObject obj) =>
        obj.Equals(this) || expression.ContainsAny(obj);
}