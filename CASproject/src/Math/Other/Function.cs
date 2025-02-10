namespace CAS;

/// <summary>
/// Represents a function
/// </summary>
public class Function : MathObject, NamedObject {
    public readonly string name;
    public readonly Dictionary<string,MathObject> inputs = new();
    public Function(FunctionDefinition func) : this(func,func.inputs.Select(n => new Variable(n)).ToArray()) {}
    public Function(FunctionDefinition func, MathObject[] inputs) {
        if(inputs.Length!=func.inputs.Count()) throw new Exception("Cannot create function with new inputs as input counts do not match!");
        name = func.name;
        int i = 0;
        foreach(var key in func.inputs) {
            this.inputs[key] = inputs[i];
            i++;
        }
    }

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        if(definedObjects.TryGetValue(name, out MathObject? expr)) return expr.Evaluate(inputs.Where(kvp => (kvp.Value as Variable)?.name!=kvp.Key).ToDictionary()).Evaluate(definedObjects);
        foreach(var key in inputs.Keys) 
            inputs[key] = inputs[key].Evaluate(definedObjects);
        return this;
    }

    public MathObject Simplify() {
        foreach(var key in inputs.Keys) 
            inputs[key] = inputs[key].Simplify();
        return this;
    }

    public bool Equals(MathObject obj) =>
        obj is Function fun &&           //same type
        fun.name==name &&                //same name
        fun.inputs.Keys.All(key => this.inputs[key].Equals(fun.inputs[key]));//same inputs

    public bool ContainsAny(MathObject obj) => obj.Equals(this) || inputs.Values.Any(n => n.ContainsAny(obj));
    public string AsString() => name+"("+string.Join(";",inputs.Values.Select(n => n.AsString()))+")";
    public string GetName() => name;
}