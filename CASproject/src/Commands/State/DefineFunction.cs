using CAS;
using Application;

namespace Commands;

public class DefineFunction : ExecutableCommand {
    public object Execute() {
        var func = new FunctionDefinition(name,inputs,function);
        Program.Define(name,func);
        return "succesfully defined function "+func.GetFunctionString()+" as "+function.AsString();
    }

    private readonly MathObject function;
    private readonly string name;
    private readonly string[] inputs;
    public DefineFunction(string name, string[] inputs, MathObject function) {
        if(inputs.Contains(name)) throw new Exception("Self-reference: Functions cannot have themselves as inputs!");
        if (function.ContainsAny(new Variable(name))) throw new Exception("Self-reference: Functions cannot be defined with themselves!");
        string? temp = Program.AllowedObjectName(name);
		if(temp!=null) throw new Exception(temp);
		this.name = name;
        this.inputs = inputs;
        this.function = function;
    }
}