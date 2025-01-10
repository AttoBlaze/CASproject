using CAS;

namespace Commands;
public class SimplifyExpression : ExecutableCommand {
    public object Execute() => expression.Simplify();

    private readonly MathObject expression;
    public SimplifyExpression(MathObject expression) {
        this.expression = expression;
    }
}