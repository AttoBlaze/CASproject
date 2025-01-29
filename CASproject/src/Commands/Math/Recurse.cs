using CAS;

public sealed class Recurse : MathCommand {
	private readonly MathObject expression = (Constant)0;
	private readonly string[] variables = [];
	private readonly MathObject[] initialValues = [];
	private readonly MathObject recursions = (Constant)0;
	public Recurse(IEnumerable<string> variables, IEnumerable<MathObject> initialVals, MathObject recursions, MathObject expression) {
		this.variables = variables.ToArray();
		this.initialValues = initialVals.ToArray();
		this.recursions = recursions;
		this.expression = expression;
	}

    public override MathObject Evaluate(Dictionary<string, MathObject> definedObjects) =>
		new Recurse(variables,initialValues.Select(n => n.Evaluate(definedObjects)),recursions.Evaluate(definedObjects),expression).execute();

    public override MathObject execute() {
		var dict = new Dictionary<string,MathObject>();
		for(int i=0;i<variables.Length;i++) dict[variables[i]] = (Constant)0;
		for(int i=0;i<Math.Min(initialValues.Length,variables.Length);i++) dict[variables[i]] = initialValues[i];

		//recursively evaluate
		MathObject value = initialValues[0];
		for(int i=0;i<recursions.Calculate().AsValue();i++) {
			value = expression.Evaluate(dict);
			for(int j=variables.Length-1;j>0;j--) dict[variables[j]] = dict[variables[j-1]];
			dict[variables[0]] = value;
		}
		return value;
	}
    public override string AsString() {
		var inptNames = string.Join(";",variables);
		var inptInitialVals = string.Join(";",initialValues.Where((n,i)=>i<variables.Length).Select(n => n.AsString()));
		return "recurse("+
			(variables.Length>1?"("+inptNames+")":inptNames)+";"+					//variables
			(inptInitialVals.Length>1?"("+inptInitialVals+")":inptInitialVals)+";"+	//initial values
			recursions.AsString()+";"+												//recursion count
			expression.AsString()+													//recursion expression
		")";
	}
}