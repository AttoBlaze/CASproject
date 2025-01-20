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
            arguments => {
                return new HelpCommand();
        });
        new Command(
            "explain",
            "Explains a command/setting/formal function",
            [
                "SETTING","Explains the given setting",
                "COMMAND","Explains the usage of the given command",
                "FUNCTION","Explains the usage of the given formal function"
            ], 
            arguments => {
                return new ExplainCommand(arguments.Pop().AsInput());
        });
        new Command(
            "list",
            "Lists objects",
            ListObjects.listables.Keys.Select(key => new string[]{
                key, ListObjects.listables[key].description
            }).SelectMany(n=>n).ToArray(),
            arguments => {
                string name = arguments.Pop().AsInput();
                return new ListObjects(name);
		});
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
            arguments => new ExitCommand()
        );
        new Command(
            "Evaluate",
            "Evalutes the given mathematical expression without simplifying it. This will only evaluate the expression when used",
            EXPR,
            arguments => new EvaluateExpression((MathObject)arguments.Pop())
        );
        new Command(
            "evaluate",
            "Immediately evalutes the given mathematical expression without simplifying it",
            EXPR,
            arguments => ((MathObject)arguments.Pop()).Evaluate(Program.definedObjects)
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
                return new DerivativeExpression((MathObject)args[1],args[0].AsInput());
        });
        new Command(
            "derivative",
            "Immediately gets the derivative of the given mathematical expression.",
            [
                "EXPRESSION;VARIABLE","Gets the derivative of the expression relative to the given variable"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
                return ((MathObject)args[1]).Differentiate(args[0].AsInput());
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
                    obj => obj.Calculate().Differentiate(args[0].AsInput()).Simplify(),
                    (MathObject)args[1]
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
                return ((MathObject)args[1]).Calculate().Differentiate(args[0].AsInput()).Simplify();
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
                var expression = (MathObject)args[0];       //first value in stack will be the expression
                string name = args[1].AsInput();            //second in stack will be name
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
                object[] args = (object[])arguments.Pop();                                                  //multiple command inputs combine
                string[] inputs = args.Skip(1).SkipLast(1).Select(n => n.AsInput()).Reverse().ToArray();    //function inputs are all but last and first of command inputs
                string name = args.Last().AsInput();                                                        //last command input is name
                if (name.ToCharArray().Any(c => !char.IsLetter(c))) throw new Exception("Defined object names can only consist of letters!");
                return new DefineFunction(name,inputs,(MathObject)args[0]);
		});
        new Command(
            "remove",
            "Removes the definition of the given object",
            [
                "OBJECT","Removes the definition of OBJECT"
            ],
            arguments => {
                return new RemoveObject(arguments.Pop().AsInput());
        });
        new Command(
            "setSetting",
            "Sets the value of a setting",
            [
                "SETTING;INPUTS..","Sets the given setting to the given input"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
                return new SetSetting(args.Last().AsInput(),args.Count()==2?args.First():args.Skip(1).ToArray());
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
            "Immediately returns the amount of time the execution of the given command took in seconds. "+
            "Be aware that immediately executed commands are not timeable like this.",
            CMD,
            arguments => new GetTime(arguments.Pop()).Execute()
        );
    }
}