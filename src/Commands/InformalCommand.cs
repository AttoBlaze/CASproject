namespace Commands;

/// <summary>
/// Represents an informally defined command
/// </summary>
public class InformalCommand : ExecutableCommand {
    public Func<object> GetCommand() => getCommand(args);

    private readonly Func<object[],Func<object>> getCommand;
    private readonly object[] args;
    public InformalCommand(Func<object[],Func<object>> getCommand, params object[] args) {
        this.getCommand = getCommand;
        this.args = args;
    }
}