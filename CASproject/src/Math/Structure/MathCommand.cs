using CAS;
using Commands;

public abstract class MathCommand : ExecutableCommand, MathObject {
	public object Execute() => execute();
	public abstract MathObject execute();
	public abstract string AsString();
	public virtual MathObject Evaluate(Dictionary<string,MathObject> definedObjects) => execute();
	public virtual MathObject Simplify() => (MathObject)this.MemberwiseClone();
	public virtual bool Equals(MathObject obj) => false;
}