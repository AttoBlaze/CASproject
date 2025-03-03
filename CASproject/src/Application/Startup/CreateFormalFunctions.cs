namespace CAS;

using Application;
using Commands;

public sealed partial class FormalFunction {
    public static void CreateAllFormalFunctions() {
        //ln
        CreateFormalFunction(
            "ln","Natural logarithm of x",["x"],
            arguments => new Ln((MathObject)arguments.Pop()) 
        );

        //trigonometric functions
        CreateFormalFunction(
            "sin","Sin of x",["x"],
            arguments => new Sin((MathObject)arguments.Pop())
        );
        CreateFormalFunction(
            "asin","Arc sin of x / sin^-1(x)",["x"],
            arguments => new Asin((MathObject)arguments.Pop())
        );
        CreateFormalFunction(
            "cos","Cos of x",["x"],
            arguments => new Cos((MathObject)arguments.Pop())
        );
        CreateFormalFunction(
            "acos","Arc cos of x / cos^-1(x)",["x"],
            arguments => new Acos((MathObject)arguments.Pop())
        );
        CreateFormalFunction(
            "tan","Tan of x",["x"],
            arguments => new Tan((MathObject)arguments.Pop())
        );
        CreateFormalFunction(
            "atan","Arc tan of x / tan^-1(x)",["x"],
            arguments => new Atan((MathObject)arguments.Pop())
        );

        //iterative math
        CreateFormalFunction(
            "sum",
            "The sum function for the given expression\n"+
            "Evaluates each term of the sequence decided by the given expression and returns its sum.\n"+
            "The input functions as the variable on the bottom of the sum function. The until functions as the upper value in the sum function.",
            ["input","initial value","until","expression"],
            arguments => {
                var args = (object[])arguments.Pop();
                return new Sum(
                    args[0].AsInput(),  //input variable
                    (MathObject)args[1],//initial value
                    (MathObject)args[2],//until
                    (MathObject)args[3] //expression
        );});
        CreateFormalFunction(
            "product",
            "The product function for the given expression:\n"+
            "Evaluates each term of the sequence decided by the given expression and returns its product.\n"+
            "The input functions as the variable on the bottom of the product function. The until functions as the upper value in the product function.",
            ["input","initial value","until","expression"],
            arguments => {
                var args = (object[])arguments.Pop();
                return new Product(
                    args[0].AsInput(),  //input variable
                    (MathObject)args[1],//initial value
                    (MathObject)args[2],//until
                    (MathObject)args[3] //expression
		);});
        CreateFormalFunction(
            "recurse",
            "Recursively evaluates the given expression:\n"+
            "Here, the inputs are variables. These effectively act as F0, F1, F2... for the recursive sequence.\n"+
            "On each recursion iteration, each variable gets the value of the variable to its right.\n"+
            "The variables starting values are decided by the initial values. If no initial value is given for a variable, it is set to 0.\n"+
            "Fx: Fibonacci can be defined like recurse((n;m);(0;1);K;n+m). This will yield the (K+1)th fibonacci number.",
            ["(inputs..)","(initial values..)","recursion count","expression"],
            arguments => {
                var args = (object[])arguments.Pop();
                
                //inputs
                IEnumerable<string> inpts = 
                    args[0] is object[] inputList? inputList.Select(n => n.AsInput()):    //multiple input
                    [args[0].AsInput()];                                                  //single input

                //initial values
                IEnumerable<MathObject> vals = 
                    args[1] is object[] valueList? valueList.Select(n => (MathObject)n):    //single value
                    [(MathObject)args[1]];                                                  //multiple values

                //recursion count
                var recursions = (MathObject)args[2];

                //expression
                var expr = (MathObject)args[3];
                return new Recurse(inpts,vals,recursions,expr,false,Program.simplificationSettings);
        });
		CreateFormalFunction(
            "sRecurse",
            "Acts like the recurse function, but actively simplifies the result for every iteration.",
            ["(inputs..)","(initial values..)","recursion count","expression"],
            arguments => {
                var args = (object[])arguments.Pop();
                
                //inputs
                IEnumerable<string> inpts = 
                    args[0] is object[] inputList? inputList.Select(n => n.AsInput()):    //multiple input
                    [args[0].AsInput()];                                                  //single input

                //initial values
                IEnumerable<MathObject> vals = 
                    args[1] is object[] valueList? valueList.Select(n => (MathObject)n):    //single value
                    [(MathObject)args[1]];                                                  //multiple values

                //recursion count
                var recursions = (MathObject)args[2];

                //expression
                var expr = (MathObject)args[3];
                return new Recurse(inpts,vals,recursions,expr,true,Program.simplificationSettings);
        });
        
        //root finding
        CreateFormalFunction(
            "nsolve",
            "Finds a root of the given expression with respect to the given input numerically using Newton's method",
            ["input","initial value","iterations","expression"],
            arguments => {
                var args = (object[])arguments.Pop();
                var input = args[0].AsInput();          //derivative
                var val = (MathObject)args[1];          //initial value
                var iterations = (MathObject)args[2];   //iterations
                var expr = (MathObject)args[3];         //expression
                var newton = new SimplifyExpression(new Add(//newton (simplify to increase performance as unsimplified will continually expand)
                    (Variable)input,
                    new Divide(
                        Add.Negate(expr),
                        expr.Differentiate(input)
                )));
                return new FunctionWrapper(
                    ()=> "nsolve("+input+";"+val.AsString()+";"+iterations.AsString()+";"+expr.AsString()+")",
                    (objs)=> new Recurse([input],[val],iterations,newton.Evaluate(objs),true,Program.simplificationSettings).Evaluate(objs),
					(obj) => new Recurse([input],[val],iterations,newton,true,Program.simplificationSettings).Equals(obj),
					Contains: (obj) => new Recurse([input],[val],iterations,newton,true,Program.simplificationSettings).ContainsAny(obj)
				);
        });
    }
}