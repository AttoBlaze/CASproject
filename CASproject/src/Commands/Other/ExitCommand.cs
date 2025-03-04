namespace Commands;

using Application;

public class ExitCommand : ExecutableCommand {
	public ExitCommand() {}
	public object Execute() {
		Program.Log("Exiting program...");
		ConsoleStyling.ResetConsoleStyling();
		Environment.Exit(0);
		return ExecutableCommand.State.SUCCESS;
	}
}