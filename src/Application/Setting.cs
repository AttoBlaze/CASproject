using CAS;

namespace Application;

public class Setting {
        public static readonly Func<object,object> ConvertToBool = input => {
            //string
            if (input is Variable) {
                var temp = (Variable)input;
                if(temp.name.ToLower() == "true")
                    return true;
                if (temp.name.ToLower() == "false")
                    return false;
                throw new Exception("Given input is not either \"true\" or \"false\"!");
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

        public readonly string Name;
        public readonly Action<object> Set;
        public readonly Func<object,object> ConvertInput;
        public readonly Func<object> Get;
        public Setting(string name, ref object value, Func<object,object> convertInput) {
            Name = name;

            Set = (input)=>{};
            Get = ()=>null;

            ConvertInput = convertInput;
            Program.settings[name] = this;
        }
        
        public class Accessor {
            public Accessor(ref object obj) {
                foreach(var prop in obj.GetType().GetProperties()) {

                }
            }

            public void Set(object value) {
                
            }

            public object Get() {
                return null;
            }
        }
        
    }
