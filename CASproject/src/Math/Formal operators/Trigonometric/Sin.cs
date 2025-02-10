namespace CAS;

public class Sin : MathFunction {
    protected override MathObject Create(MathObject obj) => new Sin(obj);
    public Sin(MathObject expression) {
        name = "sin";
        this.expression = expression;
    }

    public override MathObject Simplify() {
        var expr = expression.Simplify();
        
        //constant
        if(expr is Constant num) return new Constant(Math.Sin(num.value));
        
        //sin(asin(x)) = x
        if(expr is Asin fun) return fun.expression;

        return new Sin(expr);
    }
    public override MathObject Differentiate(string variable) => new Multiply(new Cos(expression),expression.Differentiate(variable));
}