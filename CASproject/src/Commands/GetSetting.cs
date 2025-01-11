using Application;

namespace Commands;

public sealed class GetSetting : ExecutableCommand {
    public object Execute() {
        return setting.convertOutput(setting.get());
    }

    private readonly Setting setting;
    public GetSetting(string name) {
        setting = Setting.Get(name);
    }
}