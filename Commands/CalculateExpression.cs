using CAS;

namespace Commands;
public class EvaluateExpression : ExecutableCommand {
    public Type[][] GetOverloads() => [
        [typeof(MathObject)]
    ];
    
    public Func<object> GetCommandByInputs() =>()=>{
        return expression.Evaluate();
    };

    private readonly MathObject expression;
    public EvaluateExpression(MathObject expression) {
        this.expression = expression;
    }
}