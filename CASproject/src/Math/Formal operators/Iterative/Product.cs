using Application;

namespace CAS;

public class Product : MathCommand {
	private readonly MathObject expression = (Constant)0;
	private readonly string variable = "";
	private readonly MathObject initialValue = (Constant)0;
	private readonly MathObject until = (Constant)0;
	public Product(string variable, MathObject initialValue, MathObject until, MathObject expression) {
		this.expression = expression;
		this.variable = variable;
		this.initialValue = initialValue;
		this.until = until;
	}

    public override MathObject Evaluate(Dictionary<string, MathObject> definedObjects) =>
		new Product(variable,initialValue.Evaluate(definedObjects),until.Evaluate(definedObjects),expression).execute();

    public override MathObject execute() {
		var dict = new Dictionary<string,MathObject>();

		//starting value and ending value
		int initial = (int)initialValue.Calculate(Program.definedObjects,SimplificationSettings.Calculation).AsValue(),
			final = (int)until.Calculate(Program.definedObjects,SimplificationSettings.Calculation).AsValue();

		//product function
		List<MathObject> terms = new();
		for(int i=initial;i<=final;i++) {
			//evaluate sequence 
			dict[variable] = (Constant)i;
			terms.Add(expression.Evaluate(dict));												
		}
		return terms.Count>0? new Multiply(terms): (Constant)0;
	}
    public override string AsString() {
		return "product("+
			variable+";"+					//variables
			initialValue.AsString()+";"+	//initial values
			until.AsString()+";"+			//until
			expression.AsString()+			//expression
		")";
	}

	public bool ContainsAny(MathObject obj) =>
		obj.Equals((Variable)variable) ||
		initialValue.ContainsAny(obj) ||
		until.ContainsAny(obj) ||
		expression.ContainsAny(obj);
}