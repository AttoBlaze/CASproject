using CAS;
namespace Commands;
public class Write : ExecutableCommand {
    public Func<object> GetCommandByInputs() =>()=>{
        object temp;
        Console.WriteLine(
            //expressions
            obj is MathObject?           ((MathObject)obj).AsString():
            
            //commands
            obj is ExecutableCommand?    
                (temp=((ExecutableCommand)obj).Execute()) is MathObject?
                    ((MathObject)temp).AsString():
                    temp:
            
            //default
            obj
        );
        return 0;
    };

    private readonly object obj;
    public Write(object obj) {
        this.obj = obj;
    }
}