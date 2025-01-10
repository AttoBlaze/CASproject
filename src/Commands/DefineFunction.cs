using CAS;
using Application;

namespace Commands;

public class DefineFunction : ExecutableCommand {
    public Func<object> GetCommand() => ()=>{
        Program.Define(name,new Function(name,inputs,function));
        return "succesfully defined function "+name+" as "+function.AsString();
    };

    private readonly MathObject function;
    private readonly string name;
    private readonly string[] inputs;
    public DefineFunction(string name, string[] inputs, MathObject function) {
        this.name = name;
        this.inputs = inputs;
        this.function = function;
    }
}