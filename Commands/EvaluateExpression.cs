using CAS;

namespace Commands;
public class EvaluateExpression : ExecutableCommand {
    public Func<object> GetCommandByInputs() =>()=>{
        return expression.Evaluate(definedVariables);
    };

    private readonly MathObject expression;
    private readonly Dictionary<string,double> definedVariables;
    public EvaluateExpression(MathObject expression, Dictionary<string,double> definedVariables) {
        this.expression = expression;
        this.definedVariables = definedVariables;
    }
}