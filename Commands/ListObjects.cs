namespace Commands;

public class ListObjects : ExecutableCommand {
    public Func<object> GetCommandByInputs() =>()=> {
        if (objects=="all") Console.WriteLine(
            string.Join("\n",
                Command.definedObjects.Keys.ToList()
                .Select(n => n+": "+Command.definedObjects[n].AsString())
        ));
        return 0;
    };

    private readonly string objects;
    public ListObjects(string objects) {
        this.objects = objects;
    }
}