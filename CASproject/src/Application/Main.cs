namespace Application;

using Commands;

internal class Application {
    static void Main() {
        Program.START();

        //input reader
        while(true) {
            string? input = Console.ReadLine();
            if (input==null) continue;
            Program.TryExecute(input);
        }       
    }
}