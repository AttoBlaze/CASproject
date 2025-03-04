namespace CAS;

using Application;

/// <summary>
/// Represents a formal mathematical operation/function, such as ln(x).
/// </summary>
public sealed partial class FormalFunction {
    public static FormalFunction Get(string name) {
        if(Program.formalFunctions.TryGetValue(name, out FormalFunction? formalFunction)) return formalFunction;
        throw new Exception("Given formal function does not exist!");
    }
	/// <summary>
	/// Creates an instance of this formal function using the given input. Used for parsing.
	/// </summary>
    public readonly Func<Stack<object>,MathObject> create;
    public readonly string name, description;
    public readonly string[] inputs;
    private FormalFunction(string name, string description, string[] inputs, Func<Stack<object>,MathObject> create) {
        this.name = name;
        this.description = description;
        this.inputs = inputs;
        this.create = create;
    }
    public static void CreateFormalFunction(string name, string description, string[] inputs, Func<Stack<object>,MathObject> create) {
		var func = new FormalFunction(name,description,inputs,create);
        
		//validate
		if (Program.commands.ContainsKey(name)) throw new Exception("Could not create formal function, as a command with the same name already exists."); 
		if (Program.settings.ContainsKey(name)) throw new Exception("Could not create formal function, as a setting with the same name already exists."); 
		
		//define
		Program.formalFunctions[name] = func;
	}
}