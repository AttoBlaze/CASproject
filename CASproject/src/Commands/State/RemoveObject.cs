using Application;

namespace Commands;

public class RemoveObject : ExecutableCommand {
    public object Execute() {
		Program.Remove(name);
        return "succesfully removed object "+name+"";
    }

    private readonly string name;
    public RemoveObject(string name) {
        if(Program.preDefinedObjects.ContainsKey(name)) throw new Exception("You cannot remove a predefined object!");
		if(!Program.definedObjects.ContainsKey(name)) throw new Exception("Object \""+name+"\" does not exist!");
		this.name=name;
    }
}