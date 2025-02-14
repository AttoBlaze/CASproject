namespace CAS;

public class Atan : MathFunction {
    protected override MathObject Create(MathObject obj) => new Atan(obj);
    public Atan(MathObject expression) {
        name = "atan";
        this.expression = expression;
    }

    public override MathObject Simplify(SimplificationSettings settings) {
        var expr = expression.Simplify(settings);
        
        //constant
        if(expr is Constant num && settings.calculateConstants) return settings.calculator.atan(num);
        
        //atan(tan(x))=x
        if(expr is Tan fun) return fun.expression;

        return new Atan(expr);
    }
    public override MathObject Differentiate(string variable) => 
		new Divide(expression.Differentiate(variable),new Add(new Power(expression,(Constant)2),(Constant)1));
}