namespace CAS;

public class Cos : MathFunction {
    protected override MathObject Create(MathObject obj) => new Cos(obj);
    public Cos(MathObject expression) {
        name = "cos";
        this.expression = expression;
    }

    public override MathObject Simplify() {
        var expr = expression.Simplify();
        
        //constant
        if(expr is Constant num) return CASMath.Cos(num);
        
        //cos(acos(x))=x
        if(expr is Acos fun) return fun.expression;

        return new Cos(expr);
    }
    public override MathObject Differentiate(string variable) => new Multiply(Add.Negate(new Sin(expression)),expression.Differentiate(variable));
}