using Application;
using CAS;

namespace Commands;

public class ListObjects : ExecutableCommand {
    public static readonly Dictionary<string,ListCommand> listables = new(){
        {"all",         new("Lists everything in the program",              Write_ALL)}, 
        {"predefined",  new("Lists all pre-defined objects in the program", Write_PREDEFINED)},
        {"objects",     new("Lists all defined objects",                    Write_MATH)},
        {"variables",   new("Lists all defined variables",                  Write_VARIABLES)},
        {"functions",   new("Lists all defined functions",                  Write_FUNCTIONS)},
        {"constants",   new("Lists all defined constants",                  Write_CONSTANTS)},
        {"commands",    new("Lists all commands in the program",            Write_COMMANDS)},
        {"settings",    new("Lists all settings in the program",            Write_SETTINGS)},
        {"formal",      new("Lists all formal functions in the program",    Write_FORMAL)}
    };

    public struct ListCommand {
        public string description;
        public Action write;
        public ListCommand(string description, Action write) {
            this.description = description;
            this.write = write;
        }
    }
    
    public object Execute() {
        if(listables.TryGetValue(objects, out ListCommand list)) list.write();                         
        return ExecutableCommand.State.SUCCESS;
    }

    private static void Write_ALL() {
        const int barsize = 50;
        Program.Log(CenteredInBar("Settings",barsize));
        Write_SETTINGS();
        Program.Log(CenteredInBar("Commands",barsize));
        Write_COMMANDS();
        Program.Log(CenteredInBar("Formal",barsize));
        Write_FORMAL();
        Program.Log(CenteredInBar("Objects",barsize));
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
    private static void Write_FUNCTIONS() {
        Program.Log(string.Join("\n",Program.formalFunctions.Keys.Select(FormalFunction.Get).Select(n => n.name+"("+string.Join(",",n.inputs)+")")));
        WriteMath(Program.GetFunctions());
    }
    private static void Write_CONSTANTS() => WriteMath(Program.GetConstants());
    private static void Write_COMMANDS() => WriteCommands(Program.GetCommands());
    
    private static void Write_FORMAL() {
        Program.Log(string.Join("\n",Program.formalFunctions.Keys.Select(FormalFunction.Get).Select(n => n.name+"("+string.Join(",",n.inputs)+")")));
    }
    private static void WriteSettings(IEnumerable<string> settings) {
        Program.Log(string.Join("\n",settings.OrderBy(n=>n)));
    }

    private static void WriteCommands(IEnumerable<string> commands) {
        Program.Log(string.Join("\n",commands.OrderBy(n=>n)));
    }

    private static void WriteMath(IEnumerable<string> objs) {
        Program.Log(string.Join("\n",objs.Select(n => 
            Program.definedObjects[n] is FunctionDefinition fun?
                fun.GetFunctionString()+": "+fun.expr.AsString():
                n + ": "+Program.definedObjects[n].AsString()
        )));
    }

    private readonly string objects;
    public ListObjects(string objects) {
        this.objects = objects;
    }
}