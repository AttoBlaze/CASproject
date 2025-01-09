namespace Commands;

public class ListObjects : ExecutableCommand {
    public Func<object> GetCommandByInputs() =>()=> {
        Console.WriteLine(string.Join("\n",(
            objects=="all"?         Command.definedObjects.Keys.ToList():
            objects=="variables"?   Command.GetVariables():
            objects=="constants"?   Command.GetConstants():
            objects=="functions"?   Command.GetFunctions():
            Array.Empty<string>()
        ).Select(n => n+": "+Command.definedObjects[n].AsString())));
        return 0;
    };

    private readonly string objects;
    public ListObjects(string objects) {
        this.objects = objects;
    }
}