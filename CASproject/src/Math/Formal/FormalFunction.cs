namespace CAS;

using Application;

public sealed partial class FormalFunction {
    public static FormalFunction Get(string name) {
        if(Program.formalFunctions.TryGetValue(name, out FormalFunction? formalFunction)) return formalFunction;
        throw new Exception("Given formal function does not exist!");
    }

    public readonly Func<Stack<object>,MathObject> create;
    public readonly string name, description;
    public readonly string[] inputs;
    public readonly bool mathematical;
    public FormalFunction(string name, string description, string[] inputs, Func<Stack<object>,MathObject> create, bool mathematical = true) {
        this.name = name;
        this.description = description;
        this.inputs = inputs;
        this.create = create;
        this.mathematical = mathematical;
        Program.formalFunctions[name] = this;
    }
}