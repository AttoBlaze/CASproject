namespace CAS;

/// <summary>
/// Represents a variable expression.
/// </summary>
public class Variable : MathObject, NamedObject {
    public readonly string name;
    public Variable(string name) {
        this.name = name;
    }

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
		//if a definition exists for this variable, then replace it with its definition (evaluated).
        if (definedObjects.TryGetValue(name, out MathObject? value)) {
			if(value.ContainsAny(this)) return value;
			return value.Evaluate(definedObjects);
		}
        return new Variable(name);
    }

    public MathObject Simplify(SimplificationSettings settings) {
        //variables cannot be simplified
        return new Variable(name);
    }

    public MathObject Differentiate(string variable, CalculusSettings settings) {
        if(variable==this.name) return new Constant(1d);
        return new Constant(0d);
    }

    public bool Equals(MathObject obj) =>
        obj is Variable &&           //same type
        ((Variable)obj).name==name;  //same name

    public string AsString() => name;

    public string GetName() => name;
    
    public static implicit operator Variable(string str) => new Variable(str);
}