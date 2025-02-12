namespace CAS;

public static partial class CASMath {
	public static Constant Log(Constant c1) => Log(c1,UseDouble);
	public static Constant Log(Constant c1, bool useDouble) =>
		useDouble?	new Constant(Math.Log(c1.doubleValue)):
					new Constant(factory.Ln(c1.decimalValue));
	
	public static Constant Exp(Constant c1) => Exp(c1,UseDouble);
	public static Constant Exp(Constant c1, bool useDouble) =>
		useDouble?	new Constant(Math.Exp(c1.doubleValue)):
					new Constant(factory.Exp(c1.doubleValue));
}