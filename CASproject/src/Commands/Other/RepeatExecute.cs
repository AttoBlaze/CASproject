using Application;

namespace Commands;

public class RepeatExecute : ExecutableCommand {
    public object Execute() {
		var results = new object[amount];
		for(int i=0;i<amount;i++) results[i] = cmd.Execute();
		return results;
    }

    private readonly ExecutableCommand cmd;
	private readonly int amount;
    public RepeatExecute(ExecutableCommand cmd, int amount) {
		this.cmd = cmd;
		this.amount = amount;
    }
}