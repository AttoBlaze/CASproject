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

            obj is Function?
                ((Function)obj).expression.AsString():

            //expressions
            obj is MathObject? ((MathObject)obj).AsString():
            
            //commands
            obj is ExecutableCommand?    
                (temp=((ExecutableCommand)obj).Execute()) is MathObject?    ((MathObject)temp).AsString():
                
                //command returns SUCCESS = nothing to write
                (nothingToWrite = temp is ExecutableCommand.State && (ExecutableCommand.State)temp==ExecutableCommand.State.SUCCESS)? null:
                
                temp.ToString():
            
            //default
            obj.ToString()
        )??"";
        if(nothingToWrite) return 0;
        
        //if AlwaysShowWrite is enabled, override the ShowAllMessages setting temporarily
        if(Program.AlwaysShowWrite) {
            bool mute = Program.MuteOutput;
            Program.MuteOutput = false;  
            Program.Log(str);
            Program.MuteOutput = mute;  
        }

        else Program.Log(str);
        return ExecutableCommand.State.SUCCESS;
    }

    private readonly object obj;
    public Write(object Obj) {
        if (Obj is Write)
            this.obj = ((Write)Obj).obj;
        else
            this.obj = Obj;
    }
}