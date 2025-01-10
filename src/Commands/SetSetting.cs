using Application;

namespace Commands;

public class SetSetting : ExecutableCommand {
    public Func<object> GetCommand() => ()=>{
        return 0;
    };

}