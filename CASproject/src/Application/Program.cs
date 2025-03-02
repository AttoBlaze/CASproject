namespace Application;
using CAS;
using Commands;

public static partial class Program {
    public static bool
        MuteOutput = false,
        MuteErrors = false,
        AlwaysWrite = true,
        AlwaysShowWrite = true;
	public static CASMath Calculator = new();
	public static SimplificationSettings simplificationSettings = new() {
		calculateConstants = true,
		eIsEulersNumber = true,
		expandParentheses = false,
		calculator = Calculator
	};
	public static CalculusSettings calculusSettings = new(){
		eIsEulersNumber = true
	};

    /// <summary>
    /// Parses and executes the given input  
    /// </summary>
    public static void Execute(string input, bool muteOutput = false, bool ensureStarted = true) {
        if(ensureStarted && !STARTED) START();

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
    public static void ExecuteAll(IEnumerable<string> inputs, bool muteOutput = false, bool ensureStarted = true) {
        foreach(var input in inputs)
            Execute(input,muteOutput,ensureStarted);
    }
    /// <inheritdoc cref="ExecuteAll(IEnumerable{string},bool)"/>
    public static void Execute(params string[] inputs) => ExecuteAll(inputs);
    
    /// <summary>
    /// Attempts to parse and execute the given input 
    /// </summary>
    public static void TryExecute(string input, bool muteOutput = false, bool ensureStarted = true) {
        bool showMsgs = MuteOutput;
        if (muteOutput) MuteOutput = true;
        
        try {
            Execute(input,muteOutput,ensureStarted);
        } catch (Exception e) {
            Log("Unknown error occured",e);
        }

        if (muteOutput) MuteOutput = showMsgs;
    }
    /// <summary>
    /// Attempts to executes all of the given inputs 
    /// </summary>
    public static void TryExecuteAll(IEnumerable<string> inputs, bool muteOutput = false, bool ensureStarted = true) {
        foreach(var input in inputs)
            TryExecute(input,muteOutput,ensureStarted);
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
	public static readonly Dictionary<string,MathObject> preDefinedObjects = new();
	
	/// <summary>
	/// Contains all defined variables (fx e, pi, x if user defined).
	/// </summary>
	public readonly static Dictionary<string,MathObject> definedObjects = new();
	public static void Define(string name, MathObject expression) {
		if (preDefinedObjects.ContainsKey(name)) throw new Exception("You cannot redefine predefined objects!");
        if (formalFunctions.ContainsKey(name)) throw new Exception("You cannot define an object with the same name as a formal function!"); 
		if (settings.ContainsKey(name)) throw new Exception("You cannot define an object with the same name as a setting!");
		if (commands.ContainsKey(name)) throw new Exception("You cannot define an object with the same name as a command!");
		
		var no = ObjectNameDisallowed(name);
		if(no!=null) throw new Exception(no);
		
		definedObjects[name] = expression;
	}
    public static void Predefine(string name, MathObject expression) {
        var no = ObjectNameDisallowed(name);
		if(no!=null) throw new Exception(no);
		
		preDefinedObjects[name] = expression;
        definedObjects[name] = expression;
    }

	/// <summary>
	/// Tests if the given name would be allowed for a defined object
	/// </summary>
	/// <returns>null if the name is allowed, otherwise a string explaining why the name is not allowed.</returns>
	public static string? ObjectNameDisallowed(string name) {
		if(!char.IsLetter(name.First()) && name.First()!='_') return "Defined object names must start with a letter or underscore!";
		if(name.Any(c => !char.IsLetterOrDigit(c) && c!='_')) return "Defined object names can only consist of letters, digits, and underscores!";
		return null;
	}

    public static IEnumerable<string> GetPredefined() => preDefinedObjects.Keys;
	public static IEnumerable<string> GetFunctions() => definedObjects.Keys.Where(key => definedObjects[key] is FunctionDefinition);
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
        var time = GetTime.Time(()=>{
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
            "Creating predefined objects.... "
        );
        Program.CreateAllPredefined();
        });
        if(!muted) WRITE(
            "Finished",
            "Startup completed in "+time+" seconds.",
            BAR,
            "Type \"help()\" for help."
        );
        STARTED = true;
    }
    private static void WRITE(params object[] args) {
        foreach(var msg in args) {
            string str = msg.ToString()??" ";
            Console.Write(str.Length>0 && str.EndsWith(' ')?str.Substring(0,str.Length-1):str+"\n");
        }
    }
}

