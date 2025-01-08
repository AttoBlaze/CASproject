namespace Commands;

public class DefineVariable : ExecutableCommand {
    public Type[][] GetOverloads() => [
        [typeof(string),typeof(double)]
    ];

    public Func<object> GetCommandByInputs() => ()=>{
        Command.DefineVariable(name,value);
        return "succesfully defined variable "+name+" as "+value;
    };

    private readonly double value;
    private readonly string name;
    public DefineVariable(string variableName, double value) {
        name = variableName;
        this.value = value;
    }


}