namespace CAS;

public static partial class CASMath{
	public static Constant Add(Constant c1, Constant c2) => Add(c1,c2,UseDouble);
	public static Constant Add(Constant c1, Constant c2, bool useDouble) =>
		useDouble?	new Constant(c1.doubleValue+c2.doubleValue):
					new Constant(factory.Add(c1.decimalValue,c2.decimalValue));
	
	public static Constant Subtract(Constant c1, Constant c2) => Subtract(c1,c2,UseDouble);
	public static Constant Subtract(Constant c1, Constant c2, bool useDouble) =>
		useDouble?	new Constant(c1.doubleValue-c2.doubleValue):
					new Constant(factory.Sub(c1.decimalValue,c2.decimalValue));
		
	public static Constant Multiply(Constant c1, Constant c2) => Multiply(c1,c2,UseDouble);
	public static Constant Multiply(Constant c1, Constant c2, bool useDouble) =>
		useDouble? 	new Constant(c1.doubleValue*c2.doubleValue):
					new Constant(factory.Mul(c1.decimalValue,c2.decimalValue));
		
	public static Constant Divide(Constant c1, Constant c2) => Divide(c1,c2,UseDouble);
	public static Constant Divide(Constant c1, Constant c2, bool useDouble) =>
		useDouble? 	new Constant(c1.doubleValue/c2.doubleValue):
					new Constant(factory.Div(c1.decimalValue,c2.decimalValue));

	public static Constant Pow(Constant c1, Constant c2) => Pow(c1,c2,UseDouble);
	public static Constant Pow(Constant c1, Constant c2, bool useDouble) =>
		useDouble?	new Constant(Math.Pow(c1.doubleValue,c2.doubleValue)):
					new Constant(factory.Pow(c1.decimalValue,c2.decimalValue));
	
}