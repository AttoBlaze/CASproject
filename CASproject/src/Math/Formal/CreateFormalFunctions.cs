namespace CAS;

using Application;
using Commands;

public sealed partial class FormalFunction {
    public static void CreateAllFormalFunctions() {
        new FormalFunction(
            "ln",
            "Natural logarithm of x",
            ["x"],
            arguments => new Ln((MathObject)arguments.Pop()) 
        );

        //trigonometric functions
        new FormalFunction(
            "sin","Sin of x",["x"],
            arguments => new Sin((MathObject)arguments.Pop())
        );
        new FormalFunction(
            "asin","Arc sin of x / sin^-1(x)",["x"],
            arguments => new Asin((MathObject)arguments.Pop())
        );
        new FormalFunction(
            "cos","Cos of x",["x"],
            arguments => new Cos((MathObject)arguments.Pop())
        );
        new FormalFunction(
            "acos","Arc cos of x / cos^-1(x)",["x"],
            arguments => new Acos((MathObject)arguments.Pop())
        );
        new FormalFunction(
            "tan","Tan of x",["x"],
            arguments => new Tan((MathObject)arguments.Pop())
        );
        new FormalFunction(
            "atan","Arc tan of x / tan^-1(x)",["x"],
            arguments => new Atan((MathObject)arguments.Pop())
        );
    }
}