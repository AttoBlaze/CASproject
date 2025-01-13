namespace Application;

public sealed partial class Setting {
    public static void CreateAllSettings() {
        string[] BOOL = ["true|false|1|0",""];
        new Setting(
            "MuteOutput",
            "Mutes command outputs from being written in the console",
            BOOL,
            ()=> Program.MuteOutput,
            (input)=> {Program.MuteOutput = (bool)input;},
            ConvertToBool
        );
        new Setting(
            "MuteErrors",
            "Mutes error logs being written in the console",
            BOOL,
            ()=> Program.MuteErrors,
            (input)=> {Program.MuteErrors = (bool)input;},
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