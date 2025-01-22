namespace Commands;

using System.Diagnostics;
using CAS;

public sealed class GetTime : ExecutableCommand {
	private static readonly Stopwatch timer = new();
	public static double Time(Action action) {
		timer.Restart();
		action();
		timer.Stop();
		return timer.Elapsed.TotalSeconds;
	}
	public object Execute() => (Constant)Time(()=>{cmd.Execute();});
	private readonly ExecutableCommand cmd;
	public GetTime(object cmd) {
		if(cmd is MathWrapper wrapper) {
			this.cmd = new InformalCommand(args => wrapper.transformation(wrapper.expression));
		}
		else if (cmd is not ExecutableCommand CMD) throw new Exception("Given input is neither an executable command nor transformable to it!");
		else {
			this.cmd = CMD;
		}
	}
}