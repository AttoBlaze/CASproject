using Application;
using CAS;

namespace Commands;

public sealed class ListObjects : ExecutableCommand {
    public static readonly Dictionary<string,ListCommand> listables = new(){
        {"all",         new("Lists everything in the program",Write_ALL)}, 
        {"predefined",  new("Lists all pre-defined objects in the program",Write_PREDEFINED)},
        {"objects",     new("Lists all defined objects",Write_MATH)},
        {"variables",   new("Lists all defined variables",Write_VARIABLES)},
        {"functions",   new("Lists all defined functions",Write_FUNCTIONS)},
        {"constants",   new("Lists all defined constants",Write_CONSTANTS)},
        {"commands",    new("Lists all commands in the program",Write_COMMANDS)},
        {"settings",    new("Lists all settings in the program",Write_SETTINGS)}
    };

    public struct ListCommand{
        public string description;
        public Action write;
        public ListCommand(string description, Action write) {
            this.description = description;
            this.write = write;
        }
    }
    
    public object Execute() {
        if(listables.TryGetValue(objects, out ListCommand list)) list.write();                         
        return 0;
    }

    private static void Write_ALL() {
        const int barsize = 50;
        Program.Log(CenteredInBar("Settings",barsize));
        Write_SETTINGS();
        Program.Log("\n"+CenteredInBar("Commands",barsize));
        Write_COMMANDS();
        Program.Log("\n"+CenteredInBar("Objects",barsize));
        Write_MATH();
        Program.Log(CenteredInBar("",barsize));
    }

    /// <summary>
    /// creates a string where a string is centered in a bar. (fx (command,11) -> --command--)
    /// </summary>
    private static string CenteredInBar(string s, int len) {
        if(s.Length>len) return s;
        string barHalf = new string(new byte[(len-s.Length)/2].Select(n => '-').ToArray());
        return barHalf+s+barHalf+((len-s.Length)%2==0?"":"-");
    }

    private static void Write_SETTINGS() => WriteSettings(Program.GetSettings());
    private static void Write_MATH() => WriteMath(Program.GetDefinedObjects());
    private static void Write_PREDEFINED() => WriteMath(Program.GetPredefined());
    private static void Write_VARIABLES() => WriteMath(Program.GetVariables());
    private static void Write_FUNCTIONS() => WriteMath(Program.GetFunctions());
    private static void Write_CONSTANTS() => WriteMath(Program.GetConstants());
    private static void Write_COMMANDS() => WriteCommands(Program.GetCommands());
    
    private static void WriteSettings(IEnumerable<string> settings) {
        Program.Log(string.Join("\n\n",settings.Select(Setting.Get).Select(setting => {
            //name
            string str = setting.name+"\n"+

            //description
            " ^-> Description: "+setting.description;

            //input args
            //overloads
            str += "\n ^-> ";
            if (setting.overloads.Length>0) {
                string[] overload = setting.overloads;
                
                //no overloads
                if (setting.overloads.Length<=2) 
                    str += "Input arguments: ("+overload[0]+")"+(overload[1].Length>0?" - "+overload[1]:"");
                
                //multiple overloads
                else {
                    str += "Input arguments:\n" + string.Join("\n",overload.Chunk(2).Select(n => "     ^-> "+n.First()+(n.Last().Length>0?" - "+n.Last():"")));
                }
            }
            else str += "Input arguments unspecified";
            return str;
        })));
    }

    private static void WriteCommands(IEnumerable<string> commands) {
        Program.Log(string.Join("\n\n",commands.Select(Command.Get).Select(cmd => {
            //name
            string str = cmd.name+"\n"+
            
            //description
            " ^-> Description: "+cmd.description;
            
            //overloads
            str += "\n ^-> ";
            if (cmd.overloads.Length>0) {
                string[] overload = cmd.overloads;
                
                //no overloads
                if (cmd.overloads.Length<=2) 
                    str += "Input: "+cmd.name+"("+overload[0]+") - "+overload[1];
                //multiple overloads
                else {
                    str += "Overloads:\n" + string.Join("\n",overload.Chunk(2).Select(n => "     ^-> "+cmd.name+"("+n.First()+") - "+n.Last()));
                }
            }
            else str += "Arguments unspecified";
            return str;
        })));
    }

    private static void WriteMath(IEnumerable<string> objs) {
        Program.Log(string.Join("\n",objs.Select(n => {
            string str = n;
            if(Program.definedObjects[n] is Function) str += ((Function)Program.definedObjects[n]).GetParameters(); //add function parameters
            str += ": "+Program.definedObjects[n].AsString();                                                                                                               //definition
            return str;
        })));      
    }

    private readonly string objects;
    public ListObjects(string objects) {
        this.objects = objects;
    }
}