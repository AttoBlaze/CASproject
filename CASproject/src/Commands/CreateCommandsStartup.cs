using CAS;
using Application;

namespace Commands;

public sealed partial class Command {
    public static void CreateAllCommands() {
        string[] 
            EXPR = ["EXPRESSION",""],
            CMD = ["COMMAND",""],
            NONE = ["",""];

        new Command(
            "help",
            "Gives information about the usage of this program",
            NONE,
            arguments => new HelpCommand()
        );
        new Command(
            "explain",
            "Explains a command/setting/formal function",
            [
                "SETTING","Explains the given setting",
                "COMMAND","Explains the usage of the given command",
                "FUNCTION","Explains the usage of the given formal function"
            ], 
            arguments => new ExplainCommand(arguments.Pop().AsInput())
        );
        new Command(
            "list",
            "Lists objects",
            ListObjects.listables.Keys.Select(key => new string[]{
                key, ListObjects.listables[key].description
            }).SelectMany(n=>n).ToArray(),
            arguments => new ListObjects(arguments.Pop().AsInput())
        );
        new Command(
            "write",
            "Writes the output of the given command",
            CMD,
            arguments => new Write(arguments.Pop())
        );
		new Command(
            "exit",
            "Exits the application", 
            NONE,
            arguments => new InformalCommand(args => {
                Environment.Exit(0);
                return ExecutableCommand.State.SUCCESS;
        }));
        new Command(
            "Evaluate",
            "Evalutes the given expression without simplifying it. This will only evaluate the expression when used. Can evaluate certain commands.",
            EXPR,
            arguments => new EvaluateExpression((MathObject)arguments.Pop())
        );
        new Command(
            "evaluate",
            "Immediately evalutes the given expression without simplifying it. Can evaluate certain commands.",
            EXPR,
            arguments => ((Evaluatable<MathObject>)arguments.Pop()).Evaluate(Program.definedObjects)
        );
        new Command(
            "Simplify",
            "Simplifies the given mathematical expression. This will only simplify when the expression is used",
            EXPR,
            arguments => new SimplifyExpression((MathObject)arguments.Pop())
        );
        new Command(
            "simplify",
            "Immediately simplifies the given mathematical expression",
            EXPR,
            arguments => ((MathObject)arguments.Pop()).Simplify()
        );
        new Command(
            "Calculate",
            "Calculates the given mathematical expression. This will only calculate when the expression is used",
            EXPR,
            arguments => new CalculateExpression((MathObject)arguments.Pop())
        );
        new Command(
            "calculate",
            "Immediately calculates the given mathematical expression",
            EXPR,
            arguments => ((MathObject)arguments.Pop()).Calculate()
        );
        new Command(
            "Derivative",
            "Gets the derivative of the given mathematical expression. This will only derive when the expression is used",
            [
                "EXPRESSION;VARIABLE","Gets the derivative of the expression relative to the given variable"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
                return new DerivativeExpression((MathObject)args[0],args[1].AsInput());
        });
        new Command(
            "derivative",
            "Immediately gets the derivative of the given mathematical expression.",
            [
                "EXPRESSION;VARIABLE","Gets the derivative of the expression relative to the given variable"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
                return ((MathObject)args[0]).Differentiate(args[1].AsInput());
        });
        new Command(
            "Diff",
            "Gets the simplified derivative of the given mathematical expression after evalutation. This will only derive when the expression is used",
            [
                "EXPRESSION;VARIABLE","Gets the derivative of the expression relative to the given variable"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
                return new InformalMathWrapper(
                    "Diff",
                    obj => obj.Diff(args[1].AsInput()),
                    (MathObject)args[0]
                );
        });
        new Command(
            "diff",
            "Immediately gets the simplified derivative of the given mathematical expression after evalutation.",
            [
                "EXPRESSION;VARIABLE","Gets the derivative of the expression relative to the given variable"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
                return ((MathObject)args[0]).Diff(args[1].AsInput());
        });
        new Command(
            "define",
            "Defines a math object",
            [
                "NAME;EXPRESSION","Defines a variable",
                "NAME;INPUTS..;EXPRESSION","Defines a function with the given inputs"
            ],
            arguments => {
                //variables
                if (((object[])arguments.Peek()).Length==2)
                    return Command.Get("defineVariable").create(arguments);
                
                //functions
                return Command.Get("defineFunction").create(arguments);
		});
        new Command(
            "defineVariable",
            "Defines a variable",
            [
                "NAME;EXPRESSION","Defines a variable with the given name and expression"
            ],
            arguments => {
                object[] args = (object[])arguments.Pop();
                string name = args[0].AsInput();
                var expression = (MathObject)args[1];
                if (name.ToCharArray().Any(c => !char.IsLetter(c))) throw new Exception("Defined object names can only consist of letters!");
                return new DefineVariable(name,expression);
		});
		new Command(
            "defineFunction",
            "Defines a function",
            [
                "NAME;INPUTS..;EXPRESSION","Defines a function with the given name, inputs, and expression"
            ],
            arguments => {
                object[] args = (object[])arguments.Pop();
                string[] inputs = args.Skip(1).SkipLast(1).Select(n => n.AsInput()).ToArray();    //function inputs are all but last and first of command inputs
                string name = args[0].AsInput();
                if (name.ToCharArray().Any(c => !char.IsLetter(c))) throw new Exception("Defined object names can only consist of letters!");
                return new DefineFunction(name,inputs,(MathObject)args.Last());
		});
        new Command(
            "remove",
            "Removes the definition of the given object",
            [
                "OBJECT","Removes the definition of OBJECT"
            ],
            arguments => new RemoveObject(arguments.Pop().AsInput())
        );
        new Command(
            "setSetting",
            "Sets the value of a setting",
            [
                "SETTING;INPUTS..","Sets the given setting to the given input"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
                return new SetSetting(args[0].AsInput(),args.Count()==2?args[1]:args.Skip(1).ToArray());
        });
        new Command(
            "getSetting",
            "Gets the value of the specified setting. Note: value will be copied as is, and will not update when setting changes",
            [
                "SETTING","Gets the value of the given setting. The gotten value is not updated when the setting changes."
            ],
            arguments => {
                var setting = Setting.Get(arguments.Pop().AsInput());
                return setting.convertOutput(setting.get());
        });
        new Command(
            "hide",
            "Mutes the program output during execution of the given command",
            CMD,
            arguments => new HideCommand(arguments.Pop())
        );
        new Command(
            "show",
            "Unmutes the program output during execution of the given command",
            CMD,
            arguments => new ShowCommand(arguments.Pop())
        );
        new Command(
            "time",
            "Immediately returns the amount of time the execution of the given command took in seconds. \n"+
            "NOTE: Immediately executed commands are not timeable.",
            CMD,
            arguments => new GetTime(arguments.Pop()).Execute()
        );
        new Command(
            "Time",
            "Gets the amount of time the execution of the given command took in seconds. \n"+
            "NOTE: Immediately executed commands are not timeable.",
            CMD,
            arguments => new GetTime(arguments.Pop())
        );

        new Command(
            "recurse",
            "Recursively evaluates the given expression",
            [
                "(INPUTS..);(INITIAL VALUES..);RECURSION COUNT;EXPRESSION",
                    "Recursively evaluates the given expression.\n"+
                    "Here, the inputs are variables. These effectively act as F0, F1, F2...\n"+
                    "On each recursion iteration, each variable gets the value of the current value of the variable to its right\n"+
                    "The variables starting values are decided by the initial values. If no initial value is given for a variable, it is set to 0.\n"+
                    "Fx: Fibonacci can be defined like recurse((n;m);(0;1);K;n+m). This will yield the (K+1)th fibonacci number."
            ],
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
            }
        );
        new Command(
            "product",
            "The product function for the given expression",
            [
                "INPUT;INITIAL VALUE;UNTIL;EXPRESSION",
                    "Evaluates each term of the sequence decided by the given expression and returns its product.\n"+
                    "The input functions as the variable on the bottom of the product function. The until functions as the upper value in the product function."
            ],
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
            }
        );
    }
}