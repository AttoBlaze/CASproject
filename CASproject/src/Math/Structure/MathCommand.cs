using CAS;
using Commands;

/// <summary>
/// A simultanious mathobject and executeable command.
/// </summary>
public abstract class MathCommand : ExecutableCommand, MathObject {
	public object Execute() => execute();
	public abstract MathObject execute();
	public abstract string AsString();
	public virtual MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => execute();
	public virtual MathObject Simplify(SimplificationSettings settings) => (MathObject)this.MemberwiseClone();
	public virtual bool Equals(MathObject obj) => false;
	public virtual bool ContainsAny(MathObject obj) => Equals(obj);
}