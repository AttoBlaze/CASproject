using Application;

namespace Commands;

public sealed class ExplainCommand : ExecutableCommand {
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
        
        //commands
        if (Program.commands.ContainsKey(thing)) {
            return ExplainCmd(Command.Get(thing)); 
        }

        throw new Exception("Cannot explain given input as it is not a command/setting!");
    }

    public static string ExplainSetting(Setting setting) {
        //name
        string str = setting.name+"\n"+

        //description
        " ^-> Description: "+setting.description;

        //input args/overloads
        str += "\n ^-> ";
        if (setting.overloads.Length>0) {
            string[] overload = setting.overloads;
            
            //no overloads
            if (setting.overloads.Length<=2) 
                str += "Input arguments: ("+overload[0]+")"+(overload[1].Length>0?" - "+overload[1]:"");
            
            //multiple overloads
            else {
                str += "Input arguments:\n" + string.Join("\n",overload.Chunk(2).Select(n => "     ^-> "+n.First()+(n.Last().Length>0?" - "+n.Last():"")));
            }
        }
        else str += "Input arguments unspecified";
        return str;
    }

    public static string ExplainCmd(Command cmd) {
        //name
        string str = cmd.name+"\n"+
        
        //description
        " ^-> Description: "+cmd.description;
        
        //overloads
        str += "\n ^-> ";
        if (cmd.overloads.Length>0) {
            string[] overload = cmd.overloads;
            
            //no overloads
            if (cmd.overloads.Length<=2) 
                str += "Input: "+cmd.name+"("+overload[0]+") "+(overload[1].Length>0?" - "+overload[1]:"");
            //multiple overloads
            else {
                str += "Overloads:\n" + string.Join("\n",overload.Chunk(2).Select(n => "     ^-> "+cmd.name+"("+n.First()+") - "+n.Last()));
            }
        }
        else str += "Arguments unspecified";
        return str;
    }
}