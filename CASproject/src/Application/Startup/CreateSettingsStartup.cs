using CAS;

namespace Application;

public sealed partial class Setting {
    public static void CreateAllSettings() {
        string[] 
			BOOL = ["true|false|1|0",""],
			INTEGER = ["INTEGER","","EXPRESSION","Expression must be calculateable to an integer"];
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
		new Setting(
            "ArbitraryPrecision",
            "When enabled, calculations will be made using doubles. When disabled, calculations are instead made with arbitrary precision.",
            BOOL,
            ()=> Program.Calculator.arbitraryPrecision,
            (input)=> {Program.Calculator.arbitraryPrecision = (bool)input;},
            ConvertToBool
        );
		new Setting(
			"Precision",
			"The level of precision used when calculating with arbitrary precision. Input will be clamped to the range 5 to 1000.",
			INTEGER,
			()=> Program.Calculator.precision,
			(input) => {
				var val = (long)input;
				Program.Calculator.precision = val;
				if(val>16) {
					Program.Predefine("e",Program.Calculator.e);
					Program.Predefine("pi",Program.Calculator.pi);
				}
			},
			ConvertToLong
		);
		
    }
}