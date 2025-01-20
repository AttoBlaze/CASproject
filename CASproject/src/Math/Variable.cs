namespace CAS;

/// <summary>
/// Represents a variable value
/// </summary>
public class Variable : MathObject, NamedObject {
    public string name {get; private set;}
    public Variable(string name) {
        this.name = name;
    }

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        if (definedObjects.TryGetValue(name, out MathObject? value)) return value.Evaluate(definedObjects);
        return this;
    }

    public MathObject Simplify() {
        //variables cannot be simplified
        return this;
    }

    public MathObject Differentiate(string variable) {
        if(variable==this.name) return new Constant(1);
        return new Constant(0);
    }

    public bool Equals(MathObject obj) =>
        obj is Variable &&           //same type
        ((Variable)obj).name==name;  //same name

    public string AsString() => name;

    public string GetName() => name;
    
    public static implicit operator Variable(string str) => new Variable(str);
}