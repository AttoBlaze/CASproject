namespace Commands;

using System.Diagnostics;
using CAS;

public sealed class GetTime : ExecutableCommand {
	public object Execute() {
		var timer = new Stopwatch();
		timer.Start();
		cmd.Execute();
		timer.Stop();
		return (Constant)timer.Elapsed.TotalSeconds;
	} 

	private readonly ExecutableCommand cmd;
	public GetTime(object cmd) {
		if(cmd is MathWrapper wrapper)
			this.cmd = new InformalCommand(args => wrapper.transformation(wrapper.expression));
		else if (cmd is not ExecutableCommand CMD) throw new Exception("Given input is neither an executable command nor transformable to it!");
		else this.cmd = CMD;
	}
}