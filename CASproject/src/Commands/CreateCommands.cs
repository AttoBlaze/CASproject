using CAS;
using Application;
using System.Linq;

namespace Commands;

public sealed partial class Command {
    public static void CreateAllCommands() {
         new Command(
            "evaluate",
            "Evalutes the given mathematical expression without simplifying it",
            arguments => new EvaluateExpression((MathObject)arguments.Pop(),Program.definedObjects)
        );
        new Command(
            "write",
            "Writes the output of the given command",
            arguments => new Write(arguments.Pop())
        );
        new Command(
            "simplify",
            "Simplifies the given mathematical expression",
            arguments => new SimplifyExpression((MathObject)arguments.Pop())
        );
        new Command(
            "calculate",
            "Calculates the given mathematical expression",
            arguments => new InformalCommand(args =>()=> ((MathObject)args[0]).Calculate(Program.definedObjects??new()),arguments.Pop())
        );
		new Command(
            "exit",
            "Exits the application", 
            arguments => new ExitCommand()
        );
        new Command(
            "define",
            "Defines a math object",
            [
                "NAME;EXPRESSION","Defines a variable",
                "NAME;INPUTS..;EXPRESSION","Defines a function with the given inputs"
            ],
            arguments => {
                if((Program.formalDefinedObjects??new()).ContainsKey(((object[])arguments.Peek()).Last().AsInput()))
                    throw new Exception("You cannot redefine formally defined objects!");

                //variables
                if (((object[])arguments.Peek()).Length==2)
                    return Command.Get("defineVariable").create(arguments);
                
                //functions
                return Command.Get("defineFunction").create(arguments);
		});
        new Command(
            "defineVariable",
            "Defines a variable",
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
            arguments => {
                object[] args = (object[])arguments.Pop();                                       //multiple command inputs combine
                string[] inputs = args.Skip(1).SkipLast(1).Select(n => n.AsInput()).ToArray();   //function inputs are all but last and first of command inputs
                string name = args.Last().AsInput();                                             //last command input is name
                if (name.ToCharArray().Any(c => !char.IsLetter(c))) throw new Exception("Defined object names can only consist of letters!");
                return new DefineFunction(name,inputs,(MathObject)args[0]);
		});
        new Command(
            "list",
            "Lists objects",
            ListObjects.listables.Keys.Select(key => new string[]{
                key,ListObjects.listables[key].description
            }).SelectMany(n=>n).ToArray(),
            arguments => {
                string name = arguments.Pop().AsInput();
                return new ListObjects(name);
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
    }
}