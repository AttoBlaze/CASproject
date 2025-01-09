using CAS;

namespace Commands;

public class DefineVariable : ExecutableCommand {
    public Func<object> GetCommandByInputs() => ()=>{
        Command.Define(name,new Constant(value));
        return "succesfully defined variable "+name+" as "+value;
    };

    private readonly double value;
    private readonly string name;
    public DefineVariable(string variableName, double value) {
        name = variableName;
        this.value = value;
    }
}