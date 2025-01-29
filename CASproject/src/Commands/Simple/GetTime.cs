namespace Commands;

using System.Diagnostics;
using CAS;

public sealed class GetTime : MathCommand {
	private static readonly Stopwatch timer = new();
	public static double Time(Action action) {
		timer.Restart();
		action();
		timer.Stop();
		return timer.Elapsed.TotalSeconds;
	}
	public override MathObject execute() => (Constant)Time(()=>{cmd.Execute();});
	public override string AsString() => "Time("+str+")";

	private readonly ExecutableCommand cmd;
	private readonly string str;
	public GetTime(object cmd) {
		if(cmd is MathWrapper wrapper) {
			this.cmd = new InformalCommand(args => wrapper.transformation(wrapper.expression));
			this.str = wrapper.AsString();
		}
		else if (cmd is not ExecutableCommand CMD) throw new Exception("Given input is neither an executable command nor transformable to it!");
		else {
			this.cmd = CMD;
			this.str = CMD.GetType().FullName??"UNKNOWN";
		}
	}
}