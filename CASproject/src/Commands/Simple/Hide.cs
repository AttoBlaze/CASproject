namespace Commands;

using Application;

public class HideCommand : ExecutableCommand {
	public object Execute() {
		bool muted = Program.MuteOutput;
		Program.MuteOutput = true;		
		var result = cmd.Execute();
		Program.MuteOutput = muted;
		return result;
	} 

	private readonly ExecutableCommand cmd;
	public HideCommand(object cmd) {
		this.cmd = Command.ConvertOutputToCommand(cmd);
	}
}