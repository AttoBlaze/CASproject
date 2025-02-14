namespace CAS;

/// <summary>
/// Represents a wrapper for a function. Fx nsolve, which is a wrapper for the recurse command. 
/// </summary>
public class FunctionWrapper : MathObject {
	private readonly Func<string> asString;
    private readonly Func<Dictionary<string,MathObject>,MathObject> evaluate;
	private readonly Func<SimplificationSettings,MathObject> simplify; 
	private readonly Func<MathObject,bool> equals, contains;
	
	public FunctionWrapper(Func<string> AsString, Func<Dictionary<string,MathObject>,MathObject> Evaluate, Func<MathObject,bool>? Equals = null, Func<SimplificationSettings,MathObject>? Simplify = null, Func<MathObject,bool>? Contains = null) {
		asString = AsString;
		evaluate = Evaluate;
		simplify = Simplify ?? ((s)=>evaluate(new()).Simplify(s));
		equals =   Equals 	?? ((obj)=>false);
		contains = Contains ?? equals;
	}

	public MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => evaluate(definedObjects);
	public MathObject Simplify(SimplificationSettings settings) => simplify(settings);	
	public string AsString() => asString();
	public bool Equals(MathObject obj) => equals(obj);
	public bool ContainsAny(MathObject obj) => contains(obj);
}