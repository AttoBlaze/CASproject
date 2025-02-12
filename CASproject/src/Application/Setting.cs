using CAS;
using Commands;
using DecimalSharp;

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
    public readonly Func<object,MathObject> convertOutput;
    public readonly Func<object> get;
    
    public Setting(string name, string description, string[] overloads, Func<object> get, Action<object> set, Func<object,object> convertInput) : this(name,description,overloads,get,set,convertInput,GetOutputConverter(get())) {}
    public Setting(string name, string description, string[] overloads, Func<object> get, Action<object> set, Func<object,object> convertInput, Func<object,MathObject> convertOutput) {
        this.name = name;
        this.description = description;
        this.overloads = overloads;
        this.convertInput = convertInput;
        this.convertOutput = convertOutput;
        this.set = set;
        this.get = get;
        Program.settings[name] = this;
    }
    
    public static Func<object,MathObject> GetOutputConverter(object value) {
        if(value is string) return val => new Variable((string)val);
        if(value.IsNumericType()) return val => new Constant(val.ToString()??"");
        if(value is bool) return val => new Constant((bool)val?1d:0d);
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
                if (temp.AsValue()==0) return false;
                if (temp.AsValue()==1) return true;
            throw new Exception("Given expression is not simplifiable to a constant value (where 1=true, 0 = false)!");
        }
        
        throw new Exception("Unable to convert input to a bool");
    }; 

	public static readonly Func<object,object> ConvertToLong = input => {
        //value 
        if (input is MathObject math) {
            var temp = math.Calculate();
            if (temp is Constant c && c.AsValue()%1==0 && c.AsValue()<=long.MaxValue && c.AsValue()>=long.MinValue) 
				return (long)c.AsValue();
			throw new Exception("Given input does not result in an integer value when calculated");
        }
        
        throw new Exception("Unable to convert input to an integer");
    }; 
}
