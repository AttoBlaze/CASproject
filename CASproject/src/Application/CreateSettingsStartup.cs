namespace Application;

public sealed partial class Setting {
    public static void CreateAllSettings() {
        string[] BOOL = ["true|false|1|0",""];
        new Setting(
            "ShowAllMessages",
            "Enables/disables logs being written in the console",
            BOOL,
            ()=> Program.ShowAllMessages,
            (input)=> {Program.ShowAllMessages = (bool)input;},
            ConvertToBool
        );
        new Setting(
            "ShowAllErrors",
            "Enables/disables error logs being written in the console",
            BOOL,
            ()=> Program.ShowAllErrors,
            (input)=> {Program.ShowAllErrors = (bool)input;},
            ConvertToBool
        );
        new Setting(
            "AlwaysWrite",
            "Enables/disables all executed commands being written in the console",
            BOOL,
            ()=> Program.AlwaysWrite,
            (input)=> {Program.AlwaysWrite = (bool)input;},
            ConvertToBool
        );
        new Setting(
            "AlwaysShowWrite",
            "When enabled, the Write command will override the ShowAllMessages setting and will always be written in the console",
            BOOL,
            ()=> Program.AlwaysShowWrite,
            (input)=> {Program.AlwaysShowWrite = (bool)input;},
            ConvertToBool
        );
    }
}