
namespace CAS;

/// <summary>
/// Represents a definition for a function (fx: f(x) -> x^2).
/// </summary>
public class FunctionDefinition : MathObject {
    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) => expr.Evaluate(definedObjects);
    public MathObject Simplify(SimplificationSettings settings) => expr.Simplify(settings);
    public string AsString() => expr.AsString();
    public bool Equals(MathObject obj) => expr.Equals(obj);
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