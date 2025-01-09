using CAS;

namespace Commands;

public class DefineVariable : ExecutableCommand {
    public Func<object> GetCommandByInputs() => ()=>{
        Command.Define(name,value);
        return "succesfully defined variable "+name+" as "+value;
    };

    private readonly MathObject value;
    private readonly string name;
    public DefineVariable(string variableName, MathObject value) {
        name = variableName;
        var result = value.Calculate(Command.definedObjects);
        if (result is Constant)
            this.value = result;
        else
            this.value = value;
    }
}