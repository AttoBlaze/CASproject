using CAS;
using Commands;

namespace Application;

public class Setting {
    public static Setting Get(string name) {
        if (Program.settings.TryGetValue(name, out Setting? setting)) return setting;
        throw new Exception("Setting \""+name+"\" does not exist!");
    }

    public static void CreateAllSettings() {
        new Setting(
            "HideAllMessages",
            "Disables all messages from being written in the console",
            [
                "true|false",""
            ],
            ()=> Program.HideAllMessages,
            (input)=> {Program.HideAllMessages = (bool)input;},
            ConvertToBool
        );
        new Setting(
            "HideAllErrors",
            "Disables all errors from being written in the console",
            [
                "true|false",""
            ],
            ()=> Program.HideAllErrors,
            (input)=> {Program.HideAllErrors = (bool)input;},
            ConvertToBool
        );
    }

    public readonly string name, description;
    public readonly string[] overloads;
    public readonly Action<object> set;
    public readonly Func<object,object> convertInput;
    public readonly Func<object> get;
    
    public Setting(string name, string description, string[] overloads, Func<object> get, Action<object> set, Func<object,object> convertInput) {
        this.name = name;
        this.description = description;
        this.overloads = overloads;
        this.convertInput = convertInput;
        this.set = set;
        this.get = get;
        Program.settings[name] = this;
    }
    
    /* WIP to only pass ref and not getter and setter
    public Setting(string name, ref object value, Func<object,object> convertInput) {
        Name = name;

        value.GetType().GetMember

        Set = (input)=>{};
        Get = ()=>null;

        ConvertInput = convertInput;
        Program.settings[name] = this;
    } 
    */      

    public static readonly Func<object,object> ConvertToBool = input => {
        //string
        if (input is Variable or Function) {
            var name = input.AsInput();
            if(name.ToLower() is "true")
                return true;
            if (name.ToLower() is "false")
                return false;
            throw new Exception("Given input is not either \"true\" or \"false\"! ("+name+")");
        }

        //value 
        if (input is MathObject) {
            var temp = ((MathObject)input).Calculate(Program.definedObjects);
            if (temp is Constant)
                return ((Constant)temp).value==1;
            throw new Exception("Given expression is not simplifiable to a constant value (where 1=true, !1 = false)!");
        }
        
        throw new Exception("Unable to convert input to a bool");
    }; 
}
