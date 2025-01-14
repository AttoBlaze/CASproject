using Application;

namespace Commands;

public sealed class RemoveObject : ExecutableCommand {
    public object Execute() {
		Program.definedObjects.Remove(name);
        return ExecutableCommand.State.SUCCESS;
    }

    private readonly string name;
    public RemoveObject(string name) {
        if(Program.preDefinedObjects.ContainsKey(name)) throw new Exception("You cannot remove a predefined object!");
		if(!Program.definedObjects.ContainsKey(name)) throw new Exception("Object \""+name+"\" does not exist!");
		this.name=name;
    }
}