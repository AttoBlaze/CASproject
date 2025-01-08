using System.Collections;
using CAS;

public class Operator {
    public static Dictionary<char,Operator> operators = new(){
        {'+',new Operator(
            '+',1,
            inputs => {
                var term2 = (MathObject)inputs.Pop();
                var term1 = (MathObject)inputs.Pop();
                return new Add([term1,term2]);
            }
        )},

        {'-',new Operator(
            '-',1,
            inputs => {
                var term2 = (MathObject)inputs.Pop();
                var term1 = (MathObject)inputs.Pop();
                return new Add([term1,Add.Negate(term2)]);
            }
        )},
        
        {'*',new Operator(
            '*',2,
            inputs => {
                var term2 = (MathObject)inputs.Pop();
                var term1 = (MathObject)inputs.Pop();
                return new Multiply([term1,term2]);
            }
        )}
    };

    public static int Precedence(string op) => 
        op.Length>1? 0:
        Precedence(op[0]);
    
    public static int Precedence(char op) {
        if (operators.TryGetValue(op, out Operator? ope)) return ope.precedence;
        return 0; 
    }

    /// <summary>
    /// The symbol denoting the operator
    /// </summary>
    public readonly char symbol;
    
    /// <summary>
    /// The precedence of the operator. The sign of the precedence signifies the operators association; <\b>
    /// - is left association, + is right association 
    /// </summary>
    public readonly int precedence;

    /// <summary>
    /// The operation of this operator
    /// </summary>
    public readonly Func<Stack<object>,MathObject> operation;
    public Operator(char symbol, int precedence, Func<Stack<object>,MathObject> operation) {
        this.symbol = symbol;
        this.precedence = precedence;
        this.operation = operation;
    }
}