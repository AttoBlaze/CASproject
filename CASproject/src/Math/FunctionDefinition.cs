
namespace CAS;


public class FunctionDefinition : MathObject, NamedObject {
    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) => expr.Evaluate(definedObjects);
    public MathObject Simplify() => expr.Simplify();
    public string AsString() => expr.AsString();
    public bool Equals(MathObject obj) => expr.Equals(obj);
	public string GetName() => name;
	public string GetFunctionString() => name+"("+string.Join(";",inputs)+")";
	public readonly string name;
	public readonly string[] inputs;
	public readonly MathObject expr;
	public FunctionDefinition(string name, string[] inputs, MathObject expr) {
		this.name = name;
		this.inputs = inputs;
		this.expr = expr;
	}
}