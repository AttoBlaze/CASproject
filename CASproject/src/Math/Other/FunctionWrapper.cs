namespace CAS;

public class FunctionWrapper : MathObject {
	private readonly Func<string> asString;
    private readonly Func<Dictionary<string,MathObject>,MathObject> evaluate;
	private readonly Func<MathObject> simplify; 
	private readonly Func<MathObject,bool> equals, contains;
	
	public FunctionWrapper(Func<string> AsString, Func<Dictionary<string,MathObject>,MathObject> Evaluate, Func<MathObject,bool>? Equals = null, Func<MathObject>? Simplify = null, Func<MathObject,bool>? Contains = null) {
		asString = AsString;
		evaluate = Evaluate;
		simplify = Simplify ?? (()=>evaluate(new()).Simplify());
		equals =   Equals 	?? ((obj)=>false);
		contains = Contains ?? equals;
	}

	public MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => evaluate(definedObjects);
	public MathObject Simplify() => simplify();	
	public string AsString() => asString();
	public bool Equals(MathObject obj) => equals(obj);
	public bool ContainsAny(MathObject obj) => contains(obj);
}