namespace CAS;

public class Tan : MathFunction {
    protected override MathObject Create(MathObject obj) => new Tan(obj);
    public Tan(MathObject expression) {
        name = "tan";
        this.expression = expression;
    }

    public override MathObject Simplify(SimplificationSettings settings) {
        var expr = expression.Simplify(settings);
        
        //constant
        if(expr is Constant num && settings.calculateConstants) return settings.calculator.tan(num);
        
        //tan(atan(x))=x
        if(expr is Atan fun) return fun.expression;

        return new Tan(expr);
    }
    public override MathObject Differentiate(string variable) => 
		new Multiply(new Add((Constant)1,new Power(new Tan(expression),(Constant)2)),expression.Differentiate(variable));
}