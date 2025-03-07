using System.Collections;
namespace CAS;

/// <summary>
/// Represents an operator (such as + or *)
/// </summary>
/// Rightwards unary operators have 0 precedence, leftwards unary (fx. factorial) have negative precedence 
public class Operator {
    public static Dictionary<char,Operator> operators = new(){
        {'+',new Operator(
            '+',2,
            inputs => {
                var term2 = (MathObject)inputs.Pop();
                var term1 = (MathObject)inputs.Pop();
                return new Add(term1,term2);
            }
        )},

		//unary -
        {'-',new Operator(
            '-',0,
            inputs => Add.Negate((MathObject)inputs.Pop())
        )},
        
        {'*',new Operator(
            '*',3,
            inputs => {
                var term2 = (MathObject)inputs.Pop();
                var term1 = (MathObject)inputs.Pop();
                return new Multiply(term1,term2);
            }
        )},

        {'/',new Operator(
            '/',3,
            inputs => {
                var term2 = (MathObject)inputs.Pop();
                var term1 = (MathObject)inputs.Pop();
                return new Divide(term1,term2);
            }
        )},

        {'^',new Operator(
            '^',-4,
            inputs => {
                var term2 = (MathObject)inputs.Pop();
                var term1 = (MathObject)inputs.Pop();
                return new Power(term1,term2);
            }
        )},
    };

    /// <summary>
    /// The precedence of the given operator
    /// </summary>
    public static int Precedence(string op) => 
        op.Length>1? 0:
        Precedence(op[0]);
    /// <inheritdoc cref="Precedence(string)"/>
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
    /// - is right association, + is left association 
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