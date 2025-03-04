using CAS;
using Application;
using System.Text;
using System.Linq.Expressions;

namespace Commands;

public interface ExecutableCommand {
	/// <summary>
	/// Executes this command
	/// </summary>
	public object Execute(); 

	public enum State {
		SUCCESS
	}
}

public partial class Command {
	private Command(string name, string description, string[] overloads, Func<Stack<object>,object> createCommand) {
		this.name = name;
		this.description = description;
		this.overloads = overloads;
		this.create = createCommand;
	}
	public static void CreateCommand(string name, string description, Func<Stack<object>,object> createCommand) => CreateCommand(name,description,[],createCommand);
	public static void CreateCommand(string name, string description, string[] overloads, Func<Stack<object>,object> createCommand) {
		var cmd = new Command(name,description,overloads,createCommand);

		//validate
		if (Program.formalFunctions.ContainsKey(name)) throw new Exception("Could not create command, as a formal function with the same name already exists."); 
		if (Program.settings.ContainsKey(name)) throw new Exception("Could not create command, as a setting with the same name already exists."); 
		
		//define
		Program.commands[name] = cmd;
	}
	public readonly string name;
	public readonly string description;
	public readonly string[] overloads;
	public readonly Func<Stack<object>,object> create;

	public static Command Get(string name) {
		if (Program.commands.TryGetValue(name, out Command? cmd)) return cmd;
		throw new Exception("Command \""+name+"\" does not exist!");
	}

	/// <summary>
	/// Parses a string input into an executable command. 
	/// </summary>
	public static ExecutableCommand Parse(string input) => ConvertOutputToCommand(Program.ParseInput(input));
	public static ExecutableCommand ConvertOutputToCommand(object output) {
		if (output is MathObject) 
			return new Write(((MathObject)output).Calculate());

		if (output is not ExecutableCommand)
			return new Write(output);
		
		return (ExecutableCommand)output;
	}

	/// <summary>
	/// Converts the given object to an input string for a command
	/// </summary>
	public static string AsInput(object obj) {
		if(obj is NamedObject) return ((NamedObject)obj).GetName();
		throw new Exception("Could not convert input \""+obj+"\" to a command input string");
	}
}

public static class CommandExtensions {
	/// <inheritdoc cref="Command.AsInput(object)"/>
	public static string AsInput(this object obj) => Command.AsInput(obj);
}