namespace Commands;

using Application;

public sealed class ShowCommand : ExecutableCommand {
	public object Execute() {
		bool muted = Program.MuteOutput;
		Program.MuteOutput = false;		
		var result = cmd.Execute();
		Program.MuteOutput = muted;
		return result;
	} 

	private readonly ExecutableCommand cmd;
	public ShowCommand(object cmd) {
		this.cmd = Command.ConvertOutputToCommand(cmd);
	}
}