namespace Commands;

/// <summary>
/// Represents an informally defined command
/// </summary>
public class InformalCommand : ExecutableCommand {
    public object Execute() => command(args);

    private readonly Func<object[],object> command;
    private readonly object[] args;
    public InformalCommand(Func<object[],object> command, params object[] args) {
        this.command = command;
        this.args = args;
    }
}