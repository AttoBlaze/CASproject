namespace Application;
using CAS;

public static class Program {
    public static bool 
        HideAllMessages = false,
        HideAllErrors = false;

    /// <summary>
    /// Contains all settings
    /// </summary>
    public static Dictionary<string,Setting> settings = new();
    public static void SetSetting(string name, object input) {
        if (!settings.TryGetValue(name, out Setting? setting)) 
            throw new Exception("Setting \""+name+"\" does not exist!");

        setting.Set(setting.ConvertInput);
        Log("succesfully set setting \""+name+"\" to "+setting.Get());
    }

    /// <summary>
    /// Logs the given message in the console
    /// </summary>
    public static void Log(object log, bool newLine = true) {
        if(!HideAllMessages)
            Console.Write(log + (newLine?"\n":""));
    }
    /// <inheritdoc cref="Log(object,bool)"/>
    public static void Log(object log, Exception e, bool newLine = true) =>
        Log(log + (HideAllErrors?"":"\n"+e),newLine);

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
	public static IEnumerable<string> GetFunctions() => definedObjects.Keys.Where(key => definedObjects[key] is Function);
    public static IEnumerable<string> GetDefinedObjects() => definedObjects.Keys;
    public static IEnumerable<string> GetConstants() =>	definedObjects.Keys.Where(key => definedObjects[key] is Constant);
	public static IEnumerable<string> GetVariables() =>	definedObjects.Keys.Where(key => !formalDefinedObjects.ContainsKey(key));
}

