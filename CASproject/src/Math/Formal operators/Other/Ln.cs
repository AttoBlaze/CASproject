namespace CAS;

public class Ln : MathFunction {
    protected override MathObject Create(MathObject obj) => new Ln(obj);
    public Ln(MathObject expression) {
        name = "ln";
        this.expression = expression;
    }

    public override MathObject Simplify() {
        var expr = expression.Simplify();
        
        //constant
        if(expr is Constant num) return CASMath.Log(num);
        
        //e
        if(expr is Variable e && e.name=="e") return new Constant(1d);

        //e^x
        if(((expr as Power)?.Base as Variable)?.name=="e") return expr.As<Power>().exponent;
        
        return new Ln(expr);
    }
    public override MathObject Differentiate(string variable) => new Divide(expression.Differentiate(variable),expression);
}