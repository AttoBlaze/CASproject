namespace Commands;
using CAS;

public sealed class Product : MathCommand {
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
		//initialize
		var dict = new Dictionary<string,MathObject>();
		int initial = (int)initialValue.Calculate().AsValue(),
			final = (int)until.Calculate().AsValue();

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