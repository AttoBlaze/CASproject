namespace CAS;

/// <summary>
/// Represents a function
/// </summary>
public class Function : MathObject, NamedObject {
    public readonly string name;
    public readonly Dictionary<string,MathObject> inputs = new();
    public readonly MathObject expression;
    public Function(Function func, MathObject[] inputs) {
        name = func.name;
        int i=0;
        foreach(var key in func.inputs.Keys) {
            this.inputs[key] = inputs[i];
        }
        expression = func.expression;
    }
    public Function(string name, string[] inputs, MathObject expression) {
        this.name = name;
        foreach(var input in inputs){
            this.inputs.Add(input,new Variable(input));
        }
        this.expression = expression;
    }

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        return expression.Evaluate(inputs).Evaluate(definedObjects);
    }

    public MathObject Simplify() {
        return expression.Evaluate(inputs).Simplify();
    }

    public bool Equals(MathObject obj) =>
        obj is Function &&           //same type
        ((Function)obj).name==name &&//same name
        ((Function)obj).inputs.Keys.All(key => this.inputs[key].Equals(((Function)obj).inputs[key]));//same inputs

    public bool EquivalentTo(MathObject obj) => throw new NotImplementedException();

    public bool Contains(MathObject obj) => obj.Equals(this) || expression.Contains(obj) || expression.Contains(new Variable(name));
    public string AsString() => name+"("+string.Join(";",inputs.Values.Select(n => n.AsString()))+")";
    public string GetName() => name;
}