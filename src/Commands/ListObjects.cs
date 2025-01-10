using Application;

namespace Commands;

public class ListObjects : ExecutableCommand {
    public Func<object> GetCommand() =>()=> {
        Program.Log(string.Join("\n",(
            objects=="all"?         Program.GetDefinedObjects():
            objects=="variables"?   Program.GetVariables():
            objects=="constants"?   Program.GetConstants():
            objects=="functions"?   Program.GetFunctions():
            Array.Empty<string>()
        ).Select(n => 
            n+Program.definedObjects[n].Parameters()+": "+Program.definedObjects[n].AsString()                                                                                                                //definition
        )));                                                   
        return 0;
    };

    private readonly string objects;
    public ListObjects(string objects) {
        this.objects = objects;
    }
}