using CAS;
using Application;

namespace Commands;

public sealed class DefineFunction : ExecutableCommand {
    public object Execute() {
        Program.Define(name,new Function(name,inputs,function));
        return "succesfully defined function "+name+" as "+function.AsString();
    }

    private readonly MathObject function;
    private readonly string name;
    private readonly string[] inputs;
    public DefineFunction(string name, string[] inputs, MathObject function) {
        if(inputs.Contains(name)) throw new Exception("Self-reference: Functions cannot have themselves as inputs!");
        if (function.Contains(new Variable(name))) throw new Exception("Self-reference: Functions cannot be defined with themselves!");
        this.name = name;
        this.inputs = inputs;
        this.function = function;
    }
}