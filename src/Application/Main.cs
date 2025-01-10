namespace Application;
using Commands;

internal class Application {
    private static void WRITE(params object[] args) {
        foreach(var msg in args)
            Console.WriteLine(msg);
    }
    static void Main() {
        WRITE(
            "",
            "----------------------",
            "Loading settings..."
        );
        //new Program.Setting("HideAllMessages",(input)=>{Program.HideAllMessages = (bool)input;}, ()=> Program.HideAllMessages,Program.Setting.ConvertToBool);
        //new Program.Setting("HideAllErrors",(input)=>{Program.HideAllErrors = (bool)input;}, ()=> Program.HideAllErrors,Program.Setting.ConvertToBool);
        WRITE(
            "Finished.",
            "Loading commands..."
        );
        Command.CreateAllCommands();
        WRITE(
            "Finished.",
            "",
            "----------------------",
            "Startup completed. Type \"help()\" to see a list of commands",
            "----------------------"
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