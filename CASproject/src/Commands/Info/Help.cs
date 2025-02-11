namespace Commands;

using Application;

public class HelpCommand : ExecutableCommand {
    public object Execute() {
        Program.Log(string.Join("\n",[
            "Type commands or expressions in the console to execute.",
            "To see a list of commands, type \"list(commands)\".",
            "To see a list of settings, type \"list(settings)\".",
            "To explain the usage of a specific command or setting, use the \"explain(COMMAND)\" or \"explain(SETTING)\" command.",
            "To exit the application, use the \"exit()\" command."
        ]));
        return ExecutableCommand.State.SUCCESS;
    }

    public HelpCommand() {}
}