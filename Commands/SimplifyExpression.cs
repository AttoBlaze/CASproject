using CAS;

namespace Commands;
public class SimplifyExpression : ExecutableCommand {
    public Type[][] GetOverloads() => [
        [typeof(MathObject)]
    ];
    
    public Func<object> GetCommandByInputs() =>()=>{
        return expression.Simplify();
    };

    private readonly MathObject expression;
    public SimplifyExpression(MathObject expression) {
        this.expression = expression;
    }
}