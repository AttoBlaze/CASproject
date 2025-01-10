namespace Application;
using Commands;

internal class Application {
    private static void WRITE(params object[] args) {
        foreach(var msg in args) {
            string str = msg.ToString()??" ";
            Console.Write(str.Length>0 && str.EndsWith(" ")?str.Substring(0,str.Length-1):str+"\n");
        }
    }

    static void Main() {
        const string BAR = "-------------------------------";
        WRITE(
            "",
            BAR,
            "Startup initiated",
            "Creating settings... "
        );
        Setting.CreateAllSettings();
        WRITE(
            "   Finished",
            "Creating commands... "
        );
        Command.CreateAllCommands();
        WRITE(
            "   Finished",
            "Startup completed",
            BAR,
            "Type \"help()\" to see a list of commands"
        );

        //input reader
        while(true) {
            string? input = Console.ReadLine();
            if (input==null) continue;
            
            //parse input and execute as command
            try {
                Command.Parse(input)?.Execute();
            } catch (Exception e) {
                Program.Log("Unknown error occured\n",e);
            }
        }       
    }
}