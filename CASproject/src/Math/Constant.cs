namespace CAS;

/// <summary>
/// Represents a constant value
/// </summary>
public class Constant : MathObject, Differentiable<MathObject> { 
    public double value {get; private set;}
    public Constant(double value) {
        this.value = value;
    }


    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        return this;
    }

    public MathObject Simplify() {
        //constants cannot be simplified
        return (Constant)value;
    }

    public MathObject Differentiate(string variable) => new Constant(0);

    public bool Equals(MathObject obj) =>
        obj is Constant &&  //same type
        ((Constant)obj).value==this.value;  //same value

    private static readonly string format = string.Join("",Enumerable.Range(0,300).Select(n=>"#"))+"0."+string.Join("",Enumerable.Range(0,300).Select(n=>"#"));
    public string AsString() => value.ToString(format);

    public double AsValue() => value;

    public static implicit operator Constant(double val) => new Constant(val);
}