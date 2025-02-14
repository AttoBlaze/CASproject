namespace CAS;

public class Asin : MathFunction {
    protected override MathObject Create(MathObject obj) => new Asin(obj);
    public Asin(MathObject expression) {
        name = "asin";
        this.expression = expression;
    }

    public override MathObject Simplify(SimplificationSettings settings) {
        var expr = expression.Simplify(settings);
        
        //constant
        if(expr is Constant num && settings.calculateConstants) return settings.calculator.asin(num);
        
        //asin(sin(x))=x
        if(expr is Sin fun) return fun.expression;

        return new Asin(expr);
    }
    public override MathObject Differentiate(string variable) => 
		new Divide(
			expression.Differentiate(variable),
			new Power(
				new Add((Constant)1,Add.Negate(new Power(expression,(Constant)2))),
				(Constant)0.5));
}