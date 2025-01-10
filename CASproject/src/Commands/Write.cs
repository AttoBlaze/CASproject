using CAS;
using Application;

namespace Commands;
public class Write : ExecutableCommand {
    public object Execute() {
        object temp;
        string str = (
            //expressions
            obj is MathObject? ((MathObject)obj).AsString():
            
            //commands
            obj is ExecutableCommand?    
                (temp=((ExecutableCommand)obj).Execute()) is MathObject?
                    ((MathObject)temp).AsString():
                    temp.ToString():
            
            //default
            obj.ToString()
        )??"";
        Program.Log(str);
        return 0;
    }

    private readonly object obj;
    public Write(object obj) {
        if (obj is Write)
            this.obj = ((Write)obj).obj;
        else
            this.obj = obj;
    }
}