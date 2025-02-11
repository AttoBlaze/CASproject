using Application;

namespace Commands;

public class ExecuteAll : ExecutableCommand {
    public object Execute() {
        return cmds.Select(n => n.Execute());
    }

    private readonly ExecutableCommand[] cmds;
    public ExecuteAll(IEnumerable<ExecutableCommand> cmds) {
		this.cmds = cmds.ToArray();
    }
}