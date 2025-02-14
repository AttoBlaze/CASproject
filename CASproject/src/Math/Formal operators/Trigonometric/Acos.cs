namespace CAS;

public class Acos : MathFunction {
    protected override MathObject Create(MathObject obj) => new Acos(obj);
    public Acos(MathObject expression) {
        name = "acos";
        this.expression = expression;
    }

    public override MathObject Simplify(SimplificationSettings settings) {
        var expr = expression.Simplify(settings);
        
        //constant
        if(expr is Constant num && settings.calculateConstants) return settings.calculator.acos(num);
        
        //acos(cos(x))=x
        if(expr is Cos fun) return fun.expression;

        return new Acos(expr);
    }
    public override MathObject Differentiate(string variable, CalculusSettings settings) => 
		Add.Negate(new Divide(
			expression.Differentiate(variable,settings),
			new Power(
				new Add((Constant)1,Add.Negate(new Power(expression,(Constant)2))),
				(Constant)0.5
			)
		));
}