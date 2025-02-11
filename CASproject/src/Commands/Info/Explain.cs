using Application;
using CAS;

namespace Commands;

public class ExplainCommand : ExecutableCommand {
    public object Execute() {
        Program.Log(Explain(input));
        return ExecutableCommand.State.SUCCESS;
    }

    private readonly string input;
    public ExplainCommand(string input) {
        this.input = input;
    }

    public static string Explain(string thing) {
        //setting
        if (Program.settings.ContainsKey(thing)) {
            return ExplainSetting(Setting.Get(thing)); 
        }
        
        //command
        if (Program.commands.ContainsKey(thing)) {
            return ExplainCmd(Command.Get(thing)); 
        }

        //formal function
        if(Program.formalFunctions.ContainsKey(thing))
            return ExplainFormalFunction(FormalFunction.Get(thing)); 

        throw new Exception("Cannot explain given input as it is not a command/setting/function!");
    }

    public static string ExplainSetting(Setting setting) {
        return new StringBranch(setting.name,[
            new StringLeaf("Description",setting.description),
            setting.overloads.Length<=0? new StringLeaf("Arguments unspecified"):
            setting.overloads.Length<=2?
                new StringLeaf("Input","("+setting.overloads[0]+")"+ (setting.overloads[1].Length==0?"":" - "+setting.overloads[1])):
                new StringBranch("Overloads",
                    setting.overloads.Chunk(2).Select(n => 
                    new StringLeaf(setting.name+"("+n.First()+")"+ (n.Count()==1?"":" - "+n.Last())))
                )
        ]).Write();
    }

    public static string ExplainCmd(Command cmd) {
        return new StringBranch(cmd.name,[
            new StringLeaf("Description",cmd.description),
            cmd.overloads.Length<=0? new StringLeaf("Arguments unspecified"):
            cmd.overloads.Length<=2?
                new StringLeaf("Input",cmd.name+"("+cmd.overloads[0]+")"+ (cmd.overloads[1].Length==0?"":" - "+cmd.overloads[1])):
                new StringBranch("Overloads",
                    cmd.overloads.Chunk(2).Select(n => 
                    new StringLeaf(cmd.name+"("+n.First()+")"+ (n.Count()==1?"":" - "+n.Last())))
                )
        ]).Write();
    }

    public static string ExplainFormalFunction(FormalFunction func) {
        if(func.description.Contains("\n")) 
            return new StringBranch(func.name,[
                new StringLeaf("Input",func.name+"("+string.Join(";",func.inputs)+")"),
                new StringLeaf("Description",func.description),
            ]).Write(); 
        return func.name+"("+string.Join(";",func.inputs)+") - "+func.description;
    }
}