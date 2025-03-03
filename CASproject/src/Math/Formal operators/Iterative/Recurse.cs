using Application;

namespace CAS;

public class Recurse : MathCommand {
	private readonly MathObject expression = (Constant)0;
	private readonly string[] variables = [];
	private readonly MathObject[] initialValues = [];
	private readonly MathObject recursions = (Constant)0;
	private readonly bool activelySimplify;
	private readonly SimplificationSettings simplificationSettings;
	public Recurse(IEnumerable<string> variables, IEnumerable<MathObject> initialVals, MathObject recursions, MathObject expression, bool activelySimplify = false, SimplificationSettings? settings = null) {
		this.variables = variables.ToArray();
		this.initialValues = initialVals.ToArray();
		this.recursions = recursions;
		this.expression = expression;
		this.activelySimplify = activelySimplify;
		this.simplificationSettings = settings ?? SimplificationSettings.Calculation;
	}

    public override MathObject Evaluate(Dictionary<string, MathObject> definedObjects) =>
		new Recurse(variables,initialValues.Select(n => n.Evaluate(definedObjects)),recursions.Evaluate(definedObjects),expression,activelySimplify,simplificationSettings).execute();

	public override MathObject execute() {
		var dict = new Dictionary<string,MathObject>();

		//initialize initial values
		for(int i=0;i<variables.Length;i++) dict[variables[i]] = (Constant)0;
		
		//map inputted initial values
		for(int i=0;i<Math.Min(initialValues.Length,variables.Length);i++) dict[variables[i]] = initialValues[i];

		MathObject value = initialValues.Last();
		for(int i=0;i<recursions.Simplify(SimplificationSettings.Calculation).AsValue();i++) {
			//recursively evaluate
			value = expression.Evaluate(dict);
			if(activelySimplify) value = value.Simplify(simplificationSettings);

			//update inputs
			for(int j=0;j<variables.Length-1;j++) dict[variables[j]] = dict[variables[j+1]];	
			dict[variables[variables.Length-1]] = value;															
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

	public new bool Equals(MathObject obj) =>
		obj is Recurse recurse &&
		recurse.expression.Equals(this.expression) &&
		MathObject.TermListsAreExactlyEqual(this.initialValues,recurse.initialValues) &&
		recurse.variables.Select((n,i) => this.variables[i]==n).All(n => n==true);

	public override bool ContainsAny(MathObject obj) => 
		obj.Equals(this) ||
		expression.ContainsAny(obj) ||
		(obj is Variable v && variables.Any(n => n==v.name)) ||
		initialValues.Any(n => n.ContainsAny(obj)) ||
		recursions.ContainsAny(obj);
}