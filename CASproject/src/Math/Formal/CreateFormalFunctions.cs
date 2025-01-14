namespace CAS;

using Application;
using Commands;

public sealed partial class FormalFunction {
    public static void CreateAllFormalFunctions() {
        new FormalFunction(
            "ln",
            "Natural logarithm of x",
            ["x"],
            arguments => {
                var expr = (MathObject)arguments.Pop();
                return new Ln(expr); 
        });
    }
}