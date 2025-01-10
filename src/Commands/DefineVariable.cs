using CAS;
using Application;

namespace Commands;

public class DefineVariable : ExecutableCommand {
    public object Execute(){
        Program.Define(name,value);
        return "succesfully defined variable "+name+" as "+value;
    }

    private readonly MathObject value;
    private readonly string name;
    public DefineVariable(string variableName, MathObject value) {
        name = variableName;
        var result = value.Calculate(Program.definedObjects);
        if (result is Constant)
            this.value = result;
        else
            this.value = value;
    }
}