using Application;

namespace CAS;

public class Ln : MathFunction {
    protected override MathObject Create(MathObject obj) => new Ln(obj);
    public Ln(MathObject expression) {
        name = "ln";
        this.expression = expression;
    }

    public override MathObject Simplify(SimplificationSettings settings) {
        var expr = expression.Simplify(settings);
        
        //constant
        if(expr is Constant num && settings.calculateConstants) return settings.calculator.log(num);
        
        //e
        if(expr is Variable e && e.name=="e" && settings.eIsEulersNumber) return new Constant(1d);

        //e^x
        if(((expr as Power)?.Base as Variable)?.name=="e" && settings.eIsEulersNumber) return expr.As<Power>().exponent;
        
        return new Ln(expr);
    }

	//ln(f)' = f'/f
    public override MathObject Differentiate(string variable, CalculusSettings settings) => new Divide(expression.Differentiate(variable,settings),expression);
}