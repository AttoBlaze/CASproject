namespace Commands;

public class ListObjects : ExecutableCommand {
    public Type[][] GetOverloads() => [
        [typeof(string)]
    ];

    public Func<object> GetCommandByInputs() =>()=> {
        if (objects=="variables") Console.WriteLine(
            string.Join("\n",
                Command.definedVariables.Keys.ToList()
                .Select(n => n+": "+Command.definedVariables[n])
        ));
        return 0;
    };

    private readonly string objects;
    public ListObjects(string objects) {
        this.objects = objects;
    }
}