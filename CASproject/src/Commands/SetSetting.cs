using Application;

namespace Commands;

public sealed class SetSetting : ExecutableCommand {
    public object Execute() {
        object before = setting.get();
        setting.set(setting.convertInput(args));
        return "succesfully set setting \""+setting.name+"\" to "+setting.get()+" (previously set to "+before+")";
    }

    private readonly Setting setting;
    private readonly object args;
    public SetSetting(string name, object args) {
        setting = Setting.Get(name);
        this.args = args;
    }
}