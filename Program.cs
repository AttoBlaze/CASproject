namespace CASproject;
using CAS;
using Commands;

internal class Program {
    static void Main(string[] args) {
        while(true) {
            string? input = Console.ReadLine();
            if (input==null) continue;
            
            //parse input and execute as command
            try {
                Command.Parse(input)?.Execute();
            } catch (Exception e) {
                Console.WriteLine("ERROR: Unknown error occured!\n"+e);
            }
        }       
    }
}


