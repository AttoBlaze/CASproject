namespace Commands;

/// <summary>
/// Represents an informally defined command
/// </summary>
public class InformalCommand : ExecutableCommand {
    public Type[][] GetOverloads() => overloads();
    
    public Func<object> GetCommandByInputs() => getCommand(args);

    private readonly Func<Type[][]> overloads;
    private readonly Func<object[],Func<object>> getCommand;
    private readonly object[] args;
    public InformalCommand(Func<Type[][]> overloads, Func<object[],Func<object>> getCommand, params object[] args) {
        this.overloads = overloads;
        this.getCommand = getCommand;
        this.args = args;
    }
}