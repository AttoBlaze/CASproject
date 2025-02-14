namespace CAS;

public class Sin : MathFunction {
    protected override MathObject Create(MathObject obj) => new Sin(obj);
    public Sin(MathObject expression) {
        name = "sin";
        this.expression = expression;
    }

    public override MathObject Simplify(SimplificationSettings settings) {
        var expr = expression.Simplify(settings);
        
        //constant
        if(expr is Constant num && settings.calculateConstants) return settings.calculator.sin(num);
        
        //sin(asin(x)) = x
        if(expr is Asin fun) return fun.expression;

        return new Sin(expr);
    }
    public override MathObject Differentiate(string variable, CalculusSettings settings) => 
		new Multiply(new Cos(expression),expression.Differentiate(variable,settings));
}