namespace Application;

using System.Drawing;
using CAS;
using Commands;

public static partial class Program {
	#region Fields
    public static bool
        MuteOutput = false,
        MuteWarnings = false,
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
	public static readonly ConsoleStyling 
		LoggerStyling = new(ConsoleFontStyling.Italic),
		LoggerWarningStyling = new(Color.Yellow),
		LoggerErrorStyling = new(Color.Red);
	#endregion

	#region Parsing + executing
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
            LogError("Error occured during command execution",e);
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
	#endregion

	#region Commands, Settings, Formal functions
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
	#endregion

	#region Logging
    /// <summary>
    /// Logs the given message in the console
    /// </summary>
    public static void Log(object log, ConsoleStyling? styling = null, bool newLine = true) {
        if(MuteOutput) return;
		styling ??= LoggerStyling;
		styling.Write(log + (newLine?"\n":""));
    }

	/// <summary>
    /// Logs the given warning in the console
    /// </summary>
    public static void LogWarning(object log, ConsoleStyling? styling = null,  bool newLine = true) {
		if(MuteWarnings) return;
		Log("WARNING: " + log, styling ?? LoggerWarningStyling, newLine);
	}

	/// <summary>
    /// Logs the given error in the console
    /// </summary>
    public static void LogError(object log, Exception e, ConsoleStyling? styling = null, bool newLine = true) {
		if(MuteErrors) return;
		Log("ERROR: " + log + ":\n" + e, styling ?? LoggerErrorStyling, newLine);
	}
	#endregion

	#region Objects
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
	public static IEnumerable<string> GetUserDefined() =>	definedObjects.Keys.Where(key => !preDefinedObjects.ContainsKey(key));
	public static IEnumerable<string> GetVariables() =>	definedObjects.Keys.Where(key => definedObjects[key] is not FunctionDefinition);
	#endregion

	#region Startup
    private static bool STARTED = false;
    /// <summary>
    /// Initiates the startup process (if startup has not been initiated yet)
    /// </summary>
    public static void START(bool muted = false) {
        if (STARTED) return;
        const string BAR = "---------------------------------------";
		ConsoleStyling 
			bar =		new(Color.White),
			start = 	new(Color.Magenta,ConsoleFontStyling.Bold,ConsoleFontStyling.Underlined),
			creating = 	new(Color.LightGray,ConsoleFontStyling.Italic), 
			dots =		new(Color.DarkGray), 
			finished = 	new(Color.LightGreen,ConsoleFontStyling.Italic),
			doneIn = 	new(Color.Magenta,ConsoleFontStyling.Bold),
			help = 		new(Color.Magenta,ConsoleFontStyling.Italic)
		;
		
		var time = GetTime.Time(()=>{
        if(!muted) {
			WRITE(bar,"",BAR);
			WRITE(start,"Startup initiated");
			WRITE(creating,"Creating settings ");
			WRITE(dots,".............. ");
		}
        Setting.CreateAllSettings();
        if(!muted) {
			WRITE(finished,"Finished");
            WRITE(creating,"Creating commands ");
			WRITE(dots,".............. ");
		};
        Command.CreateAllCommands();
        if(!muted) {
			WRITE(finished,"Finished");
			WRITE(creating,"Creating formal functions ");
			WRITE(dots,"...... ");
		} 
        FormalFunction.CreateAllFormalFunctions();
        if(!muted) {
			WRITE(finished,"Finished");
            WRITE(creating,"Creating predefined objects ");
			WRITE(dots,".... ");
		}
        Program.CreateAllPredefined();
        });
        if(!muted) {
			WRITE(finished,"Finished");
			WRITE(doneIn,"Startup completed in "+time+" seconds.");
			WRITE(bar,BAR);
			WRITE(help,"Type \"help()\" for help.");
		};
        STARTED = true;
    }
	
	/// <inheritdoc cref="WRITE(ConsoleStyling,object[])"/>
    private static void WRITE(params object[] args) => WRITE(ConsoleStyling.Current,args);
    /// <summary>
	/// writes a new line for each argument unless that argument ends with a space.
	/// </summary>
	private static void WRITE(ConsoleStyling style, params object[] args) {
        foreach(var msg in args) {
            string str = msg.ToString()??" ";
            style.Write(str.Length>0 && str.EndsWith(' ')?str.Substring(0,str.Length-1):str+"\n");
        }
    }
	#endregion
}