using CAS;
using Application;
using System.Text;

namespace Commands;

public interface ExecutableCommand {
	/// <summary>
	/// Executes this command
	/// </summary>
	public object Execute(); 
}

/// <summary>
/// Exits the application
/// </summary>
public class ExitCommand : ExecutableCommand {
	public object Execute() {
		Environment.Exit(0);
		return 0;
	}

	public ExitCommand() {}
}

public sealed partial class Command {
	public Command(string name, string description, Func<Stack<object>,ExecutableCommand> createCommand) : this(name,description,[],createCommand) {}
	public Command(string name, string description, string[] overloads, Func<Stack<object>,ExecutableCommand> createCommand) {
		this.name = name;
		this.description = description;
		this.create = createCommand;
		this.overloads = overloads;
		Program.commands[name] = this;
	}
	public readonly string name;
	public readonly string description;
	public readonly string[] overloads;
	public readonly Func<Stack<object>,ExecutableCommand> create;

	public static Command Get(string name) {
		if (Program.commands.TryGetValue(name, out Command? cmd)) return cmd;
		throw new Exception("Command \""+name+"\" does not exist!");
	}

	/// <summary>
	/// Parses a string input into an executable command. 
	/// </summary>
	/// Uses a slightly modified shunting yard algorithm.
	public static ExecutableCommand Parse(string input) => (ExecutableCommand)ParseInput(input); 
	/// <summary>
	/// Parses a string input into a math object 
	/// </summary>
	public static MathObject ParseMath(string input) => (MathObject)ParseInput(input, convertToCommand:false);
	/// <summary>
	/// Parses a string input  
	/// </summary>
	public static object ParseInput(string input, bool convertToCommand = true){
		//reformat input
		char[] tokens = input
			.Replace(" ","").Replace("\n","")		//remove spaces & line breaks
			.Replace(".",",")						//make dots and commas interchangeable
			.Replace("**","^")						//make ** equivalent to a ^ operator
			.ToCharArray();
		
		//tools
		var operators = new Stack<string>();
		var output = new Stack<object>();
		var builder = new StringBuilder();

		//parse input
		for (int i=0;i<tokens.Length;i++) {
			//parse constants
			if (tokens[i]==',' || char.IsDigit(tokens[i])) {
				
                //parse numbers
				while(i<tokens.Length && (tokens[i]==',' || char.IsDigit(tokens[i]))) {
					builder.Append(tokens[i]); 
					i++;
				}

				//check for shit term
				if (double.TryParse(builder.ToString(), out double value)) {
                    output.Push(new Constant(value));				//push to output stack
				    builder.Clear();					            //reset builder
				    i--;											//account for 'overshoot'
                } 
                else throw new Exception("Value \""+builder+"\" was unable to be parsed");
			}
			
			//parse letters
			else if (char.IsLetter(tokens[i])) {
				//if this letter borders a digit, then add a multiply
                if (i>0 && char.IsDigit(tokens[i-1])) operators.Push("*");

                //parse total length
				while (i<tokens.Length && char.IsLetter(tokens[i])) {
					builder.Append(tokens[i]);
					i++;
				}
				
				//push to variable stack if a variable (no parentheses), otherwise push to the operator stack.
                if((Program.definedObjects.TryGetValue(builder.ToString(),out MathObject? obj) && (obj is not Function)) ||	//math objects that are not functions with inputs
					i>=tokens.Length || tokens[i]!='(')										
						if (obj is Function) output.Push(obj);																//function without inputs
						else output.Push(new Variable(builder.ToString()));													//math object
				
				else operators.Push(builder.ToString());																	//commands + functions with inputs
				
				builder.Clear();//reset builder
				i--;			//account for 'overshoot'
			}
			
			//parse operators
			else if (Operator.operators.TryGetValue(tokens[i], out Operator? op)) {
				
				//continually apply operators
				while (operators.Count>0 && 				//the operator stack isnt empty
						operators.Peek()!="(" && 			//the top operator is not a left parentheses
						(Math.Abs(Operator.Precedence(operators.Peek()))>Math.Abs(op.precedence) || 						//the top operator has a higher precedence than the current operator or
						(Math.Abs(Operator.Precedence(operators.Peek()))==Math.Abs(op.precedence) && op.precedence>0))) {	//the top operator and current operator have the same precedence and the current operator is left associative.
					
                    //apply operators
					output.Push(ApplyOperator(operators.Pop(),output));
					if (output.Peek()==null) throw new Exception("Combined operators lead to empty output");
				}
				operators.Push(tokens[i]+"");
			}

            //commands with several inputs have inputs seperated by ;
            else if (tokens[i]==';') operators.Push(";");
			
			//left parentheses
			else if(tokens[i]=='(') operators.Push("(");
			
			//right parentheses
			else if (tokens[i]==')') {
				while (true) {
					//if the stack is empty then an unenclosed parentheses exists
					if (operators.Count<=0) throw new Exception("Unenclosed parentheses exists in input");
					
					//if top of stack is a left parentheses then the parentheses has been calculated
					if (operators.Peek()=="(") {
						operators.Pop();
						if (operators.Count>0 && Operator.Precedence(operators.Peek())==0 && operators.Peek()!="(") output.Push(ApplyOperator(operators.Pop(),output));	//apply functions
						break;
					}
					
					//push output
					output.Push(ApplyOperator(operators.Pop(),output));
					if (output.Peek()==null) throw new Exception("Output after evaluating parentheses is null!");
				}
			}

			//if we have neither letters, numbers, an operator, or a parentheses, then a mistake must have happened.
			else throw new Exception("Unknown error occurred when parsing input");
		}
		
		//apply remaining operators
		while (operators.Count>0) {
			if (operators.Peek()=="(") throw new Exception("An unenclosed parentheses exists!");
			if (output.Peek()==null) throw new Exception("Output after evaluating is null!");
			output.Push(ApplyOperator(operators.Pop(),output));
		}
		//if the output stack isnt a single term then an error must have happened
		if (output.Count!=1) throw new Exception("Output was unable to be combined");
		
		//return result
		object Output = output.Pop();
		if (Output is ExecutableCommand) {
			if (Output is EvaluateExpression || Output is SimplifyExpression) return new Write(Output);
			return (ExecutableCommand)Output;
		}
        if (convertToCommand) return new Write(((MathObject)Output).Calculate(Program.definedObjects));
		return Output;
	}

	private static object ApplyOperator(string op, Stack<object> output) {
        //operators
        if (op.Length==1 && Operator.operators.TryGetValue(op[0], out Operator? ope)) 
            return ope.operation(output);

		//combine multiple inputs
		if (op == ";") {
			object val1 = output.Pop();
			object val2 = output.Pop();
			if (val1 is object[])
				return ((object[])val1).Append(val2).ToArray();
			return new object[]{val1,val2};
		}

        //commands
        if (Program.commands.TryGetValue(op, out Command? command))
            return command.create(output);
        
		//functions
		if (Program.definedObjects.TryGetValue(op, out MathObject? expression) && expression is Function) {
			var function = (Function)expression;
			object args = output.Pop(); //get function inputs
			
			//multiple inputs
			if (args is object[]) {
				var Args = ((object[])args).Select(n => (MathObject)n).ToArray();
				
				//error check
				if(function.inputs.Length!=Args.Length) throw new Exception("Improper math function input count! ("+Args.Length+" inputs given, "+function.inputs.Length+" inputs required)");
				
				//function inputs
				Dictionary<string,MathObject> inputs = new();
				for(int i=0 ; i<Args.Length ; i++)
					inputs[function.inputs[i]] = Args[i];
				
				return function.Evaluate(inputs);
			}

			//single input
			else {
				//error check
				if(function.inputs.Length!=1) throw new Exception("Improper math function input count! ("+1+" inputs given, "+function.inputs.Length+" inputs required)");
				
				return function.Evaluate(new(){
					{function.inputs[0],(MathObject)args}
				}); 
			}
		}
        throw new Exception("No operator could be applied!");
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