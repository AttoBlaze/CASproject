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
        new FormalFunction(
            "getSetting",
            "Gets the value of the specified setting. Note: value will be copied as is, and will not update when setting changes",
            ["SETTING"],
            arguments => {
                var setting = Setting.Get(arguments.Pop().AsInput());
                return setting.convertOutput(setting.get());
        }, mathematical: false);
    }
}