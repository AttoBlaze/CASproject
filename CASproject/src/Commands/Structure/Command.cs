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
	public Command(string name, string description, Func<Stack<object>,object> createCommand) : this(name,description,[],createCommand) {}
	public Command(string name, string description, string[] overloads, Func<Stack<object>,object> createCommand) {
		this.name = name;
		this.description = description;
		this.overloads = overloads;
		this.create = createCommand;
		Program.commands[name] = this;
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