using CAS;

namespace Commands;

public class DefineFunction : ExecutableCommand {
    public Func<object> GetCommandByInputs() => ()=>{
        Command.Define(name,new Function(name,inputs,function));
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