namespace CAS;

/// <summary>
/// Represents a constant value
/// </summary>
public class Constant : MathObject {
    public double value {get; private set;}
    public Constant(double value) {
        this.value = value;
    }


    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        return this;
    }

    public MathObject Simplify() {
        //constants cannot be simplified
        return this;
    }

    public bool Equals(MathObject obj) =>
        obj is Constant &&  //same type
        ((Constant)obj).value==this.value;  //same value

    public bool EquivalentTo(MathObject obj) => obj.Simplify().Equals(this);

    public string AsString() => value.ToString();

    public double AsValue() => value;
}