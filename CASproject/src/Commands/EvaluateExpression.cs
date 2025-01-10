using CAS;

namespace Commands;
public class EvaluateExpression : ExecutableCommand {
    public object Execute(){
        return expression.Evaluate(definedObjects);
    }

    private readonly MathObject expression;
    private readonly Dictionary<string,MathObject> definedObjects;
    public EvaluateExpression(MathObject expression, Dictionary<string,MathObject> definedObjects) {
        this.expression = expression;
        this.definedObjects = definedObjects;
    }
}