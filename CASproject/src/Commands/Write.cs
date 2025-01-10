using CAS;
using Application;

namespace Commands;
public sealed class Write : ExecutableCommand {
    public object Execute() {
        object temp;
        bool nothingToWrite = false;
        string str = (
            //variables
            obj is Variable? 
                Program.definedObjects.ContainsKey(((Variable)obj).name)?   Program.definedObjects[((Variable)obj).name].AsString():
                ((Variable)obj).name:

            //expressions
            obj is MathObject? ((MathObject)obj).AsString():
            
            //commands
            obj is ExecutableCommand?    
                (temp=((ExecutableCommand)obj).Execute()) is MathObject?    ((MathObject)temp).AsString():
                (nothingToWrite = temp is int && (int)temp==0)?          null://command returns 0 = nothing to write
                temp.ToString():
            
            //default
            obj.ToString()
        )??"";
        if(nothingToWrite) return 0;
        
        //if AlwaysShowWrite is enabled, override the ShowAllMessages setting temporarily
        if(Program.AlwaysShowWrite) {
            bool showMsgs = Program.ShowAllMessages;
            Program.ShowAllMessages = true;  
            Program.Log(str);
            Program.ShowAllMessages = showMsgs;  
        }

        else Program.Log(str);
        return 0;
    }

    private readonly object obj;
    public Write(object Obj) {
        if (Obj is Write)
            this.obj = ((Write)Obj).obj;
        else
            this.obj = Obj;
    }
}