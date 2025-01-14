namespace Application;
using CAS;
using Commands;

public static class Program {
    public static bool
        MuteOutput = false,
        MuteErrors = false,
        AlwaysWrite = true,
        AlwaysShowWrite = true;

    /// <summary>
    /// Parses and executes the given input  
    /// </summary>
    public static void Execute(string input, bool muteOutput = false) {
        bool mute = MuteOutput;
        if (muteOutput) MuteOutput = true;
        
        //parse input and execute as command
        var cmd = Command.Parse(input);
        if(AlwaysWrite && cmd is not Write) cmd = new Write(cmd);
        cmd.Execute();

        if (muteOutput) MuteOutput = mute;
    }
    /// <summary>
    /// Executes all of the given inputs 
    /// </summary>
    public static void ExecuteAll(IEnumerable<string> inputs, bool muteOutput = false) {
        foreach(var input in inputs)
            Execute(input,muteOutput:muteOutput);
    }
    /// <inheritdoc cref="ExecuteAll(IEnumerable{string},bool)"/>
    public static void Execute(params string[] inputs) => ExecuteAll(inputs);
    
    /// <summary>
    /// Attempts to parse and execute the given input 
    /// </summary>
    public static void TryExecute(string input, bool muteOutput = false) {
        bool showMsgs = MuteOutput;
        if (muteOutput) MuteOutput = true;
        
        try {
            Execute(input);
        } catch (Exception e) {
            Log("Unknown error occured",e);
        }

        if (muteOutput) MuteOutput = showMsgs;
    }
    /// <summary>
    /// Attempts to executes all of the given inputs 
    /// </summary>
    public static void TryExecuteAll(IEnumerable<string> inputs, bool muteOutput = false) {
        foreach(var input in inputs)
            TryExecute(input,muteOutput:muteOutput);
    }
    /// <inheritdoc cref="TryExecuteAll(IEnumerable{string},bool)"/>
    public static void TryExecute(params string[] inputs) => TryExecuteAll(inputs);
    

    /// <summary>
    /// Contains all settings
    /// </summary>
    public static readonly Dictionary<string,Setting> settings = new();
    public static IEnumerable<string> GetSettings() => settings.Keys;

    /// <summary>
	/// Contains all commands in the program
	/// </summary>
    public static readonly Dictionary<string,Command> commands = new();
    public static IEnumerable<string> GetCommands() => commands.Keys;

    /// <summary>
    /// Contains all formal functions in the program
    /// </summary>
    public static readonly Dictionary<string,FormalFunction> formalFunctions = new();
    public static IEnumerable<string> GetFormalFunctions() => formalFunctions.Keys;

    /// <summary>
    /// Logs the given message in the console
    /// </summary>
    public static void Log(object log, bool newLine = true) {
        if(!MuteOutput)
            Console.Write(log + (newLine?"\n":""));
    }
    /// <inheritdoc cref="Log(object,bool)"/>
    public static void Log(object log, Exception e, bool newLine = true) =>
        Log(log + (!MuteErrors?"\n"+e:""),newLine);


    /// <summary>
	/// Contains all pre-defined variables (fx e, pi).
	/// </summary>
	public static readonly Dictionary<string,MathObject> preDefinedObjects = new(){
        {"e",new Constant(Math.E)},
        {"pi",new Constant(Math.PI)},
        {"radtodeg",new Function("radtodeg",["radians"],new Divide(new Multiply([new Variable("radians"),new Constant(180)]),new Variable("pi")))},
        {"degtorad",new Function("degtorad",["degrees"],new Divide(new Multiply([new Variable("degrees"),new Variable("pi")]),new Constant(180)))},
    };
	
	/// <summary>
	/// Contains all defined variables (fx e, pi, x if user defined).
	/// </summary>
	public static Dictionary<string,MathObject> definedObjects {get; private set;} = preDefinedObjects.ToDictionary();
	public static void Define(string name, MathObject expression) {
		if (preDefinedObjects.ContainsKey(name)) throw new Exception("You cannot redefine predefined objects!");
        if (formalFunctions.ContainsKey(name)) throw new Exception("You cannot define an object with the same name as a formal function!"); 
		definedObjects[name] = expression;
	}
    public static IEnumerable<string> GetPredefined() => preDefinedObjects.Keys;
	public static IEnumerable<string> GetFunctions() => definedObjects.Keys.Where(key => definedObjects[key] is Function);
    public static IEnumerable<string> GetDefinedObjects() => definedObjects.Keys;
    public static IEnumerable<string> GetConstants() =>	definedObjects.Keys.Where(key => definedObjects[key] is Constant);
	public static IEnumerable<string> GetVariables() =>	definedObjects.Keys.Where(key => !preDefinedObjects.ContainsKey(key));

    private static bool STARTED = false;
    /// <summary>
    /// Initiates the startup process (if startup has not been initiated yet)
    /// </summary>
    public static void START(bool muted = false) {
        if (STARTED) return;
        const string BAR = "---------------------------------------";
        if(!muted) WRITE(
            "",
            BAR,
            "Startup initiated",
            "Creating settings.............. "
        );
        Setting.CreateAllSettings();
        if(!muted) WRITE(
            "Finished",
            "Creating commands.............. "
        );
        Command.CreateAllCommands();
        if(!muted) WRITE(
            "Finished",
            "Creating formal functions...... "
        );
        FormalFunction.CreateAllFormalFunctions();
        if(!muted) WRITE(
            "Finished",
            "Startup completed",
            BAR,
            "Type \"help()\" for help."
        );
        STARTED = true;
    }
    private static void WRITE(params object[] args) {
        foreach(var msg in args) {
            string str = msg.ToString()??" ";
            Console.Write(str.Length>0 && str.EndsWith(" ")?str.Substring(0,str.Length-1):str+"\n");
        }
    }
}

