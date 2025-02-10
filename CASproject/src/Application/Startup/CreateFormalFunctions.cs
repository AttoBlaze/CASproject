namespace CAS;

using Application;
using Commands;

public sealed partial class FormalFunction {
    public static void CreateAllFormalFunctions() {
        //ln
        new FormalFunction(
            "ln","Natural logarithm of x",["x"],
            arguments => new Ln((MathObject)arguments.Pop()) 
        );

        //trigonometric functions
        new FormalFunction(
            "sin","Sin of x",["x"],
            arguments => new Sin((MathObject)arguments.Pop())
        );
        new FormalFunction(
            "asin","Arc sin of x / sin^-1(x)",["x"],
            arguments => new Asin((MathObject)arguments.Pop())
        );
        new FormalFunction(
            "cos","Cos of x",["x"],
            arguments => new Cos((MathObject)arguments.Pop())
        );
        new FormalFunction(
            "acos","Arc cos of x / cos^-1(x)",["x"],
            arguments => new Acos((MathObject)arguments.Pop())
        );
        new FormalFunction(
            "tan","Tan of x",["x"],
            arguments => new Tan((MathObject)arguments.Pop())
        );
        new FormalFunction(
            "atan","Arc tan of x / tan^-1(x)",["x"],
            arguments => new Atan((MathObject)arguments.Pop())
        );

        //iterative math
        new FormalFunction(
            "sum",
            "The sum function for the given expression\n"+
            "Evaluates each term of the sequence decided by the given expression and returns its sum.\n"+
            "The input functions as the variable on the bottom of the sum function. The until functions as the upper value in the sum function.",
            ["INPUT","INITIAL VALUE","UNTIL","EXPRESSION"],
            arguments => {
                var args = (object[])arguments.Pop();
                return new Sum(
                    args[0].AsInput(),  //input variable
                    (MathObject)args[1],//initial value
                    (MathObject)args[2],//until
                    (MathObject)args[3] //expression
        );});
        new FormalFunction(
            "product",
            "The product function for the given expression:\n"+
            "Evaluates each term of the sequence decided by the given expression and returns its product.\n"+
            "The input functions as the variable on the bottom of the product function. The until functions as the upper value in the product function.",
            ["INPUT","INITIAL VALUE","UNTIL","EXPRESSION"],
            arguments => {
                var args = (object[])arguments.Pop();
                return new Product(
                    args[0].AsInput(),  //input variable
                    (MathObject)args[1],//initial value
                    (MathObject)args[2],//until
                    (MathObject)args[3] //expression
                );
        });
        new FormalFunction(
            "recurse",
            "Recursively evaluates the given expression:\n"+
            "Here, the inputs are variables. These effectively act as F0, F1, F2...\n"+
            "On each recursion iteration, each variable gets the value of the current value of the variable to its right\n"+
            "The variables starting values are decided by the initial values. If no initial value is given for a variable, it is set to 0.\n"+
            "Fx: Fibonacci can be defined like recurse((n;m);(0;1);K;n+m). This will yield the (K+1)th fibonacci number.",
            ["(INPUTS..)","(INITIAL VALUES..)","RECURSION COUNT","EXPRESSION"],
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
                return new Recurse(inpts,vals,recursions,expr);
        });
        
        //root finding
        new FormalFunction(
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
                    (objs)=> new Recurse(
                        [input],[val],iterations,newton.Evaluate(objs)
                ).Evaluate(objs));
        });
    }
}