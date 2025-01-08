using CAS;

/// <summary>
/// Represents a variable value
/// </summary>
public class Variable : MathObject {
    public string name {get; private set;}
    public Variable(string name) {
        this.name = name;
    }

    public MathObject Evaluate(Dictionary<string, double> definedVariables) {
        if (definedVariables.TryGetValue(name, out double value)) return new Constant(value);
        return this;
    }

    public MathObject Simplify() {
        //variables cannot be simplified
        return this;
    }

    public bool Equals(MathObject obj) =>
        obj is Variable &&           //same type
        ((Variable)obj).name==name;  //same name

    public bool EquivalentTo(MathObject obj) =>
        obj.Evaluate().Equals(this);

    public string AsString() => name;
}