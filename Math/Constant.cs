using CAS;

public class Constant : MathObject {
    public double value {get; private set;}
    public Constant(double value) {
        this.value = value;
    }


    public MathObject Calculate(Dictionary<string, double> definedVariables) {
        return this;
    }

    public MathObject Simplify() {
        //constants cannot be simplified
        return this;
    }

    public bool Equals(MathObject obj) =>
        obj is Constant &&  //same type
        ((Constant)obj).value==this.value;  //same value

    public bool EquivalentTo(MathObject obj) =>
        obj.Calculate().Equals(this);

    public string AsString() => value.ToString();
}