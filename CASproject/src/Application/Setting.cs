using CAS;
using Commands;

namespace Application;

public sealed partial class Setting {
    public static Setting Get(string name) {
        if (Program.settings.TryGetValue(name, out Setting? setting)) return setting;
        throw new Exception("Setting \""+name+"\" does not exist!");
    }

    public readonly string name, description;
    public readonly string[] overloads;
    public readonly Action<object> set;
    public readonly Func<object,object> convertInput;
    public readonly Func<object,object> convertOutput;
    public readonly Func<object> get;
    
    public Setting(string name, string description, string[] overloads, Func<object> get, Action<object> set, Func<object,object> convertInput) : this(name,description,overloads,get,set,convertInput,GetOutputConverter(get())) {}
    public Setting(string name, string description, string[] overloads, Func<object> get, Action<object> set, Func<object,object> convertInput, Func<object,object> convertOutput) {
        this.name = name;
        this.description = description;
        this.overloads = overloads;
        this.convertInput = convertInput;
        this.convertOutput = convertOutput;
        this.set = set;
        this.get = get;
        Program.settings[name] = this;
    }
    
    public static Func<object,object> GetOutputConverter(object value) {
        if(value is string) return val => new Variable((string)val);
        if(value is bool) return val => new Constant((bool)val?1:0);
        throw new Exception("No settings output converter exists for type \""+value.GetType()+"\"!");
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
        if (input is NamedObject) {
            var name = input.AsInput();
            if(name.ToLower() is "true")
                return true;
            if (name.ToLower() is "false")
                return false;
            throw new Exception("Given input is not either \"true\" or \"false\"! ("+name+")");
        }

        //value 
        if (input is MathObject) {
            var temp = ((MathObject)input).Calculate();
            if (temp is Constant)
                return ((Constant)temp).value==1;
            throw new Exception("Given expression is not simplifiable to a constant value (where 1=true, !1 = false)!");
        }
        
        throw new Exception("Unable to convert input to a bool");
    }; 
}
