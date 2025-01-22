namespace Application;

using System.Text;
using CAS;
using Commands;

public static partial class Program {
	//Uses a slightly modified shunting yard algorithm.
	public static object ParseInput(string input){
		//reformat input
		char[] tokens = input
			.Replace(" ","").Replace("\n","")		//remove spaces & line breaks
			.Replace(".",",")						//make dots and commas interchangeable
			.Replace("(-","(0-")					//- in start of parentheses act as negation
			.Replace("**","^")						//make ** equivalent to a ^
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
                if((Program.definedObjects.TryGetValue(builder.ToString(),out MathObject? obj) 
					&& (obj is not FunctionDefinition)) ||															//math objects that are not functions with inputs
					i>=tokens.Length || tokens[i]!='(')										
						if (obj is FunctionDefinition) output.Push(new Function((FunctionDefinition)obj));			//function without inputs
						else output.Push(new Variable(builder.ToString()));										//math object
				
				else operators.Push(builder.ToString());														//commands + functions with inputs
				
				builder.Clear();//reset builder
				i--;			//account for 'overshoot'
			}
			
			//parse operators
			else if (Operator.operators.TryGetValue(tokens[i], out Operator? op)) {
				
				//continually apply operators
				while (operators.Count>0 && 											//the operator stack isnt empty
						operators.Peek()!="(" && 										//the top operator is not a left parentheses
						(Math.Abs(Operator.Precedence(operators.Peek()))>Math.Abs(op.precedence) || 						//the top operator has a higher precedence than the current operator or
						(Math.Abs(Operator.Precedence(operators.Peek()))==Math.Abs(op.precedence) && op.precedence>0))) {	//the top operator and current operator have the same precedence and the current operator is left associative.
					
                    //apply operators
					output.Push(ApplyOperator(operators,output));
					if (output.Peek()==null) throw new Exception("Combined operators lead to empty output");
				}
				operators.Push(tokens[i]+"");
			}

            //commands with several inputs have inputs seperated by ;
            else if (tokens[i]==';') {
				while(true) {
					if(operators.Count<=0) throw new Exception("Empty input exists!");
					
					//; and ( signify other inputs
					if(operators.Peek()=="(" || operators.Peek()==";") break;

					//push output
					output.Push(ApplyOperator(operators,output));
					if (output.Peek()==null) throw new Exception("Output after evaluating input is null!");
				}
				operators.Push(";");
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
						if (operators.Count>0 && Operator.Precedence(operators.Peek())==0 && operators.Peek()!="(") output.Push(ApplyOperator(operators,output));	//apply functions
						break;
					}
					
					//push output
					output.Push(ApplyOperator(operators,output));
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
			output.Push(ApplyOperator(operators,output));
		}
		//if the output stack isnt a single term then an error must have happened
		if (output.Count!=1) throw new Exception("Output was unable to be combined");
		
		//return result
		return output.Pop();
	}

	private static object ApplyOperator(Stack<string> operators, Stack<object> output) {
        string op = operators.Pop();

		//operators
        if (op.Length==1 && Operator.operators.TryGetValue(op[0], out Operator? ope)) 
            return ope.operation(output);

		//combine multiple inputs. This combines to make overloads possible as arguments can have variable lengths
		if (op == ";") {
			List<object> values = new(){output.Pop(),output.Pop()};
			while(operators.Peek()==";") {
				values.Add(output.Pop());
				operators.Pop();
			}	
			return values.Reverse<object>().ToArray();
		}

        //commands
        if (Program.commands.TryGetValue(op, out Command? command))
            return command.create(output);
        
		//formal functions
		if (Program.formalFunctions.TryGetValue(op, out FormalFunction? formalFunction)) 
			return formalFunction.create(output);

		//functions
		if (Program.definedObjects.TryGetValue(op, out MathObject? expression) && expression is FunctionDefinition function) {
			object args = output.Pop(); //get function inputs
			
			//multiple inputs
			if (args is object[] inputs) {
				var Args = inputs.Select(n => (MathObject)n).ToArray();
				
				//error check
				if(function.inputs.Length!=Args.Length) throw new Exception("Improper math function input count! ("+Args.Length+" inputs given, "+function.inputs.Length+" inputs required)");
				
				//function inputs
				return new Function(function,Args);
			}

			//single input
			else {
				//error check
				if(function.inputs.Length!=1) throw new Exception("Improper math function input count! ("+1+" inputs given, "+function.inputs.Length+" inputs required)");
				
				return new Function(function,[(MathObject)args]);
			}
		}
        throw new Exception("No operator/function could be applied!");
    }
}