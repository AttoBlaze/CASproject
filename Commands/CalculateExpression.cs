using CAS;

namespace Commands;
public class CalculateExpression : ExecutableCommand {
    public Type[][] GetOverloads() => [
        [typeof(MathObject)]
    ];
    
    public Func<object> GetCommandByInputs() =>()=>{
        return expression.Calculate();
    };

    private readonly MathObject expression;
    public CalculateExpression(MathObject expression) {
        this.expression = expression;
    }
}