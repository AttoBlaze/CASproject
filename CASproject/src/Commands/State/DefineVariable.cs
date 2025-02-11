using CAS;
using Application;

namespace Commands;

public class DefineVariable : ExecutableCommand {
    public object Execute(){
        Program.Define(name,value);
        return "succesfully defined variable "+name+" as "+value.AsString();
    }

    private readonly MathObject value;
    private readonly string name;
    public DefineVariable(string name, MathObject value) {
        if(value.ContainsAny(new Variable(name))) throw new Exception("Self-reference: Variables cannot be defined with themselves!");
        this.name = name;
        var result = value.Simplify();
        if (result is Constant)
            this.value = result;
        else
            this.value = value;
    }
}