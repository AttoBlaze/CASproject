namespace Application;
using CAS;
using Commands;

public static class Program {
    public static bool 
        ShowAllMessages = true,
        ShowAllErrors = true;

    /// <summary>
    /// Contains all settings
    /// </summary>
    public static Dictionary<string,Setting> settings = new();
    public static IEnumerable<string> GetSettings() => settings.Keys;
	

    /// <summary>
	/// Contains all commands in the program
	/// </summary>
    public static readonly Dictionary<string,Command> commands = new();
    public static IEnumerable<string> GetCommands() => commands.Keys;

    /// <summary>
    /// Logs the given message in the console
    /// </summary>
    public static void Log(object log, bool newLine = true) {
        if(ShowAllMessages)
            Console.Write(log + (newLine?"\n":""));
    }
    /// <inheritdoc cref="Log(object,bool)"/>
    public static void Log(object log, Exception e, bool newLine = true) =>
        Log(log + (ShowAllErrors?"\n"+e:""),newLine);

    /// <summary>
	/// Contains all pre-defined variables (fx e, pi).
	/// </summary>
	public static readonly Dictionary<string,MathObject> formalDefinedObjects = new(){
        {"e",new Constant(Math.E)},
        {"pi",new Constant(Math.PI)},
    };
	
	/// <summary>
	/// Contains all defined variables (fx e, pi, x if user defined).
	/// </summary>
	public static Dictionary<string,MathObject> definedObjects {get; private set;} = formalDefinedObjects.ToDictionary();
	public static void Define(string name, MathObject expression) {
		if (!formalDefinedObjects.ContainsKey(name)) 
			definedObjects[name] = expression;
	}
    public static IEnumerable<string> GetPredefined() => formalDefinedObjects.Keys;
	public static IEnumerable<string> GetFunctions() => definedObjects.Keys.Where(key => definedObjects[key] is Function);
    public static IEnumerable<string> GetDefinedObjects() => definedObjects.Keys;
    public static IEnumerable<string> GetConstants() =>	definedObjects.Keys.Where(key => definedObjects[key] is Constant);
	public static IEnumerable<string> GetVariables() =>	definedObjects.Keys.Where(key => !formalDefinedObjects.ContainsKey(key));

    /// <summary>
    /// Initializes the startup process
    /// </summary>
    public static void START() {
        const string BAR = "-------------------------------";
        WRITE(
            "",
            BAR,
            "Startup initiated",
            "Creating settings... "
        );
        Setting.CreateAllSettings();
        WRITE(
            "   Finished",
            "Creating commands... "
        );
        Command.CreateAllCommands();
        WRITE(
            "   Finished",
            "Startup completed",
            BAR,
            "Type \"help()\" to see a list of commands"
        );
    }
    private static void WRITE(params object[] args) {
        foreach(var msg in args) {
            string str = msg.ToString()??" ";
            Console.Write(str.Length>0 && str.EndsWith(" ")?str.Substring(0,str.Length-1):str+"\n");
        }
    }

}

