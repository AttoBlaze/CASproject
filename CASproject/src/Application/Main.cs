namespace Application;

using Commands;

internal class Application {
    static void Main() {
        Program.START();

        //input reader
        while(true) {
            string? input = Console.ReadLine();
            if (input==null) continue;
            
            //parse input and execute as command
            try {
                Command.Parse(input)?.Execute();
            } catch (Exception e) {
                Program.Log("Unknown error occured",e);
            }
        }       
    }
}