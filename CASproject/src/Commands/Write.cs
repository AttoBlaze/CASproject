using CAS;
using Application;

namespace Commands;
public sealed class Write : ExecutableCommand {
    public object Execute() {
        object temp;
        string str = (
            //variables
            obj is Variable? 
                Program.definedObjects.ContainsKey(((Variable)obj).name)?   Program.definedObjects[((Variable)obj).name].AsString():
                ((Variable)obj).name:

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
        return obj;
    }

    private readonly object obj;
    public Write(object obj) {
        if (obj is Write)
            this.obj = ((Write)obj).obj;
        else
            this.obj = obj;
    }
}