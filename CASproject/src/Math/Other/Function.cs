using Application;

namespace CAS;

/// <summary>
/// Represents a user-defined function with inputs. <br/>
/// The function expression represented is decided on evaluation, much like variables.  
/// </summary>
public class Function : MathObject, NamedObject {
    public readonly string name;
    public readonly Dictionary<string,MathObject> inputs = new();
    public Function(FunctionDefinition func) : this(func,func.inputs.Select(n => new Variable(n)).ToArray()) {}
    public Function(FunctionDefinition func, MathObject[] inputs) {
		if(inputs.Length!=func.inputs.Count()) throw new Exception("Cannot create function with new inputs as input counts do not match!");
		name = func.name;

		//insert inputs
        int i = 0;
        foreach(var key in func.inputs) {
            this.inputs[key] = inputs[i];
            i++;
        }
    }
	public Function(Function fun) {
		this.name = fun.name;
		this.inputs = fun.inputs.ToDictionary();
	}

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
        //if a definition for this function exists, replace it with its definition, evaluated using its inputs, then evaluated using all defined inputs.
		if(definedObjects.TryGetValue(name, out MathObject? expr)) 
			return expr.Evaluate(inputs).Evaluate(definedObjects);
        
		//otherwise attempt to evaluate inputs
		foreach(var key in inputs.Keys) 
            inputs[key] = inputs[key].Evaluate(definedObjects);
        
		return new Function(this);
    }

    public MathObject Simplify(SimplificationSettings settings) {
        var fun = new Function(this);
		foreach(var key in fun.inputs.Keys) 
            fun.inputs[key] = fun.inputs[key].Simplify(settings);
		return fun;
    }

	public MathObject Differentiate(string variable, CalculusSettings settings) {
		Program.LogWarning("Functions are not naturally differentiable, as their expression is unknown until evaluation. Evaluate the function in order to get its derivative.");
		return new Constant(0d);
	}

    public bool Equals(MathObject obj) =>
        obj is Function fun &&           //same type
        fun.name==name &&                //same name
        fun.inputs.Keys.All(key => this.inputs[key].Equals(fun.inputs[key]));//same inputs

    public bool ContainsAny(MathObject obj) => obj.Equals(this) || inputs.Values.Any(n => n.ContainsAny(obj));
    public string AsString() => name+"("+string.Join(";",inputs.Values.Select(n => n.AsString()))+")";
    public string GetName() => name;
}