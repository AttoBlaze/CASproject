using CAS;

namespace Commands;

public interface ExecutableCommand {
    /// <summary>
    /// Gets the list of overloads this command has
    /// </summary>
    public Type[][] GetOverloads();
    protected Func<object> GetCommandByInputs();
    public object Execute() => GetCommandByInputs()(); 
}

public class Command {
    public static readonly Dictionary<string,Func<Stack<object>,ExecutableCommand>> Commands = new(){
        //calculates the given math expression
        {"calculate",arguments => new CalculateExpression((MathObject)arguments.Pop())},
        
        //writes the given input result in the console
        {"write",arguments => new Write(arguments.Pop())},
        
        //simplifies the given expression
        {"simplify",arguments => new SimplifyExpression((MathObject)arguments.Pop())},
        
        //calculates and simplifies the given expression
        {"evaluate",arguments => new InformalCommand(
            ()=> [[typeof(MathObject)]],
            args => ()=> ((MathObject)args[0]).Calculate().Simplify(),
            arguments.Pop()
        )},

        {"ADDD",arguments => new InformalCommand(
            ()=> [[typeof(Add)]],
            args => ()=> ((MathObject)args[0]).Calculate().Simplify(),
            arguments.Pop()
        )},

    };
    public static readonly Dictionary<string,Constant> FormalConstants = new(){
        {"e",new Constant(Math.E)},
        {"pi",new Constant(Math.PI)},
    };
    
    
    public static ExecutableCommand Parse(string input) {
		//stacks
		var operators = new Stack<string>();
		var output = new Stack<object>();

		//tool variables
		string builder = "";
		char[] tokens = input.ToCharArray();
		for (int i=0;i<tokens.Length;i++) {
            //parse constants
			if (tokens[i]=='.' || Char.IsDigit(tokens[i])) {
				
                //get number as string
				while(i<tokens.Length && (tokens[i]=='.' || tokens[i]==',' || Char.IsDigit(tokens[i]))) {
					builder += tokens[i]; 
					i++;
				}

				//check for shit term
				if (Double.TryParse(builder, out double value)) {
                    output.Push(new Constant(value));				//push to output stack
				    builder = "";					                //reset builder
				    i--;											//account for 'overshoot'
                } 
                else throw new Exception("Value \""+builder+"\" was unable to be parsed");
			}
			

			//parse letters
			else if (Char.IsLetter(tokens[i])) {
				//if this letter borders a digit, then add a multiply
                if (i>0 && Char.IsDigit(tokens[i-1])) operators.Push("*");

                //get whole length
				while (i<tokens.Length && Char.IsLetter(tokens[i])) {
					builder += tokens[i];
					i++;
				}
				
				//push to variable stack if string is a variable, otherwise push to operator stack.
                if(FormalConstants.ContainsKey(builder)) output.Push(FormalConstants[builder]);
                else operators.Push(builder);

                builder = "";	//reset builder
				i--;			//account for 'overshoot'
			}
			
			//parse operators
			else if (Operator.operators.TryGetValue(tokens[i], out Operator? op)) {
				while (operators.Count>0 && 				//the operator stack isnt empty
						operators.Peek()!="(" && 			//the top operator is not a left parentheses
						(Math.Abs(Operator.Precedence(operators.Peek()))>=Math.Abs(op.precedence) || 						//the top operator has a higher precedence than the current operator or
						(Math.Abs(Operator.Precedence(operators.Peek()))==Math.Abs(op.precedence) && op.precedence<0))) {	//the top operator and current operator have the same precedence and the current operator is left associative.
					
                    //apply operators
					output.Push(ApplyOperator(operators.Pop(),output));
					if (output.Peek()==null) throw new Exception("Combined operators lead to empty output");
				}
				operators.Push(tokens[i]+"");
			}
			
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

            //commands with several inputs have inputs seperated by ;
            else if (tokens[i]==';') continue;
			
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
		if (output.Peek() is ExecutableCommand) return (ExecutableCommand)output.Pop();
        return new Write(((MathObject)output.Pop()).Calculate().Simplify());
	}

    private static object ApplyOperator(string op, Stack<object> output) {
        //operators
        if (op.Length==1 && Operator.operators.TryGetValue(op[0], out Operator? ope)) 
            return ope.operation(output);

        //commands
        if (Commands.TryGetValue(op, out Func<Stack<object>,ExecutableCommand>? createCommand))
            return createCommand(output);
        
        throw new Exception("No operator could be applied!");
    }
}