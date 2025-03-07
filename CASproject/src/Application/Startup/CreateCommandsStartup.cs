using CAS;
using Application;

namespace Commands;

public sealed partial class Command {
    public static void CreateAllCommands() {
        string[] 
            EXPR = ["EXPRESSION",""],
            CMD = ["COMMAND",""],
            NONE = ["",""];

        CreateCommand(
            "help",
            "Gives information about the usage of this program",
            NONE,
            arguments => new HelpCommand()
        );
        CreateCommand(
            "explain",
            "Explains a command/setting/formal function",
            [
                "SETTING","Explains the given setting",
                "COMMAND","Explains the usage of the given command",
                "FUNCTION","Explains the usage of the given formal function"
            ], 
            arguments => new ExplainCommand(arguments.Pop().AsInput())
        );
        CreateCommand(
            "list",
            "Lists objects",
            ListObjects.listables.Keys.Select(key => new string[]{
                key, ListObjects.listables[key].description
            }).SelectMany(n=>n).ToArray(),
            arguments => new ListObjects(arguments.Pop().AsInput())
        );
        CreateCommand(
            "write",
            "Writes the output of the given command",
            CMD,
            arguments => new Write(arguments.Pop())
        );
		CreateCommand(
            "exit",
            "Exits the application", 
            NONE,
            arguments => new ExitCommand()
		);
        CreateCommand(
            "Evaluate",
            "Evalutes the given expression without simplifying it. This will only evaluate the expression when used. Can evaluate certain commands.",
            EXPR,
            arguments => new EvaluateExpression((MathObject)arguments.Pop())
        );
        CreateCommand(
            "evaluate",
            "Immediately evalutes the given expression without simplifying it. Can evaluate certain commands.",
            EXPR,
            arguments => ((Evaluatable<MathObject>)arguments.Pop()).Evaluate(Program.definedObjects)
        );
        CreateCommand(
            "Simplify",
            "Simplifies the given mathematical expression. This will only simplify when the expression is used",
            EXPR,
            arguments => new SimplifyExpression((MathObject)arguments.Pop())
        );
        CreateCommand(
            "simplify",
            "Immediately simplifies the given mathematical expression",
            EXPR,
            arguments => ((MathObject)arguments.Pop()).Simplify()
        );
        CreateCommand(
            "Calculate",
            "Calculates the given mathematical expression. This will only calculate when the expression is used",
            EXPR,
            arguments => new CalculateExpression((MathObject)arguments.Pop())
        );
        CreateCommand(
            "calculate",
            "Immediately calculates the given mathematical expression",
            EXPR,
            arguments => ((MathObject)arguments.Pop()).Calculate()
        );
        CreateCommand(
            "Derivative",
            "Gets the partial derivative of the given mathematical expression. This will only derive when the expression is used",
            [
                "EXPRESSION;VARIABLE","Gets the partial derivative of the expression relative to the given variable"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
				if(args.Length!=2) throw new InputCountException("Derivative",2,args.Length);
                return new DerivativeExpression((MathObject)args[0],args[1].AsInput());
        });
        CreateCommand(
            "derivative",
            "Immediately gets the partial derivative of the given mathematical expression.",
            [
                "EXPRESSION;VARIABLE","Gets the partial derivative of the expression relative to the given variable"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
				if(args.Length!=2) throw new InputCountException("derivative",2,args.Length);
				return ((MathObject)args[0]).Differentiate(args[1].AsInput());
        });
        CreateCommand(
            "Diff",
            "Gets the simplified partial derivative of the given mathematical expression after evalutation. This will only derive when the expression is used",
            [
                "EXPRESSION;VARIABLE","Gets the partial derivative of the expression relative to the given variable"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
				if(args.Length!=2) throw new InputCountException("Diff",2,args.Length);
                return new DiffExpression((MathObject)args[0],args[1].AsInput());
        });
        CreateCommand(
            "diff",
            "Immediately gets the simplified partial derivative of the given mathematical expression after evalutation.",
            [
                "EXPRESSION;VARIABLE","Gets the partial derivative of the expression relative to the given variable"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
				if(args.Length!=2) throw new InputCountException("diff",2,args.Length);
                return ((MathObject)args[0]).Diff(args[1].AsInput());
        });
        CreateCommand(
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
        CreateCommand(
            "defineVariable",
            "Defines a variable",
            [
                "NAME;EXPRESSION","Defines a variable with the given name and expression"
            ],
            arguments => {
                object[] args = (object[])arguments.Pop();
                string name = args[0].AsInput();
                var expression = (MathObject)args[1];
				if(args.Length!=2) throw new InputCountException("defineVariable",2,args.Length);
                return new DefineVariable(name,expression);
		});
		CreateCommand(
            "defineFunction",
            "Defines a function",
            [
                "NAME;INPUTS..;EXPRESSION","Defines a function with the given name, inputs, and expression"
            ],
            arguments => {
                object[] args = (object[])arguments.Pop();
                if(args.Length<3) throw new InputCountException("defineFunction","3 or more",args.Length);
				string[] inputs = args.Skip(1).SkipLast(1).Select(n => n.AsInput()).ToArray();    //function inputs are all but last and first of command inputs
                string name = args[0].AsInput();
                return new DefineFunction(name,inputs,(MathObject)args.Last());
		});
        CreateCommand(
            "remove",
            "Removes the definition of the given object",
            [
                "OBJECT","Removes the definition of OBJECT"
            ],
            arguments => new RemoveObject(arguments.Pop().AsInput())
        );
        CreateCommand(
            "setSetting",
            "Sets the value of a setting",
            [
                "SETTING;INPUTS..","Sets the given setting to the given input"
            ],
            arguments => {
                var args = (object[])arguments.Pop();
				if(args.Length<2) throw new InputCountException("setSetting","at least 2",args.Length);
                return new SetSetting(args[0].AsInput(),args.Count()==2?args[1]:args.Skip(1).ToArray());
        });
        CreateCommand(
            "getSetting",
            "Gets the value of the specified setting. Note: value will be copied as is, and will not update when setting changes",
            [
                "SETTING","Gets the value of the given setting. The gotten value is not updated when the setting changes."
            ],
            arguments => {
                var setting = Setting.Get(arguments.Pop().AsInput());
                return setting.convertOutput(setting.get());
        });
        CreateCommand(
            "hide",
            "Mutes the program output during execution of the given command",
            CMD,
            arguments => new HideCommand(arguments.Pop())
        );
        CreateCommand(
            "show",
            "Unmutes the program output during execution of the given command",
            CMD,
            arguments => new ShowCommand(arguments.Pop())
        );
        CreateCommand(
            "time",
            "Immediately returns the amount of time the execution of the given command took in seconds. \n"+
            "NOTE: Immediately executed commands are not timeable.",
            CMD,
            arguments => new GetTime(arguments.Pop()).Execute()
        );
        CreateCommand(
            "Time",
            "Gets the amount of time the execution of the given command took in seconds. \n"+
            "NOTE: Immediately executed commands are not timeable.",
            CMD,
            arguments => new GetTime(arguments.Pop())
        );
        CreateCommand(
            "executeall",
            "executes all of the given commands",
            ["COMMANDS..",""],
            arguments => {
                var cmds = ((object[])arguments.Pop()).Select(n => (ExecutableCommand)n);
                return new ExecuteAll(cmds);
        });
		CreateCommand(
            "repeatexecute",
            "repeatedly executes the given command",
            ["COMMAND","COUNT"],
            arguments => {
                var args = (object[])arguments.Pop();
				if(args.Length!=2) throw new InputCountException("repeatexecute",2,args.Length);
                var cmd = (ExecutableCommand)args[0];
				var amount = (int)((MathObject)args[1]).Calculate().As<Constant>().doubleValue;
				return new RepeatExecute((ExecutableCommand)args[0],amount);
        });
		
    }
}