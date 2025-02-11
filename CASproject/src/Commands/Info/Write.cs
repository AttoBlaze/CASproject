using CAS;
using Application;
using System.Collections;

namespace Commands;
public class Write : ExecutableCommand {
    public object Execute() {
        string str = AsString(obj);
        if(str=="") return ExecutableCommand.State.SUCCESS;

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
        if (Obj is Write write)
            this.obj = write.obj;
        else this.obj = Obj;
    }

    public static string AsString(object obj) {
        //variables
        if(obj is Variable vari) 
            return Program.definedObjects.TryGetValue(vari.name, out MathObject? value)? value.AsString(): vari.name;

        //functions
        if (obj is Function fun) 
            return Program.definedObjects[fun.name].AsString();

        //expressions
        if (obj is MathObject mObj) 
            return mObj.AsString();

        //command returns SUCCESS = nothing to write
        if (obj is ExecutableCommand.State state && state==ExecutableCommand.State.SUCCESS) 
            return "";
            
        //commands
        if (obj is ExecutableCommand cmd) 
            return AsString(cmd.Execute());
            
        //list    
        if (obj is IEnumerable list && obj is not string) {
            var outputs = list.Cast<object>().Select(AsString);                     //get strings
            return string.Join("\n",outputs.Select(n => n==""?"[no output]":n));    //mark empty outputs
        }

        //default
        return obj.ToString()??"";
    }
}