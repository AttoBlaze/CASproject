namespace CAS;

/// <summary>
/// Represents a function
/// </summary>
public class Function : MathObject, NamedObject {
    public string name {get; private set;}
    public string[] inputs {get; private set;}
    public MathObject expression {get; private set;}
    public Function(string name, string[] inputs, MathObject expression) {
        this.name = name;
        this.inputs = inputs;
        this.expression = expression;
    }

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        return expression.Evaluate(definedObjects);
    }

    public MathObject Simplify() {
        return new Function(name,inputs,expression.Simplify());
    }

    public bool Equals(MathObject obj) =>
        obj is Function &&           //same type
        ((Function)obj).name==name;  //same name

    public bool EquivalentTo(MathObject obj) => throw new NotImplementedException();

    public bool Contains(MathObject obj) => obj.Equals(this) || expression.Contains(obj) || expression.Contains(new Variable(name));
    public string AsString() => expression.AsString();
    public string GetParameters() => "("+string.Join(",",inputs)+")";
    public string GetName() => name;
}