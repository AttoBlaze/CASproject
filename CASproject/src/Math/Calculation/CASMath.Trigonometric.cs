namespace CAS;

public static partial class CASMath {
	public static Constant Sin(Constant c1) => Sin(c1,UseDouble);
	public static Constant Sin(Constant c1, bool useDouble) =>
		useDouble?	new Constant(Math.Sin(c1.doubleValue)):
					new Constant(factory.Sin(c1.decimalValue));

	public static Constant Cos(Constant c1) => Cos(c1,UseDouble);
	public static Constant Cos(Constant c1, bool useDouble) =>
		useDouble?	new Constant(Math.Cos(c1.doubleValue)):
					new Constant(factory.Cos(c1.decimalValue));

	public static Constant Tan(Constant c1) => Tan(c1,UseDouble);
	public static Constant Tan(Constant c1, bool useDouble) =>
		useDouble?	new Constant(Math.Tan(c1.doubleValue)):
					new Constant(factory.Tan(c1.decimalValue));
	
	public static Constant Asin(Constant c1) => Asin(c1,UseDouble);
	public static Constant Asin(Constant c1, bool useDouble) =>
		useDouble?	new Constant(Math.Asin(c1.doubleValue)):
					new Constant(factory.Asin(c1.decimalValue));

	public static Constant Acos(Constant c1) => Acos(c1,UseDouble);
	public static Constant Acos(Constant c1, bool useDouble) =>
		useDouble?	new Constant(Math.Acos(c1.doubleValue)):
					new Constant(factory.Acos(c1.decimalValue));

	public static Constant Atan(Constant c1) => Atan(c1,UseDouble);
	public static Constant Atan(Constant c1, bool useDouble) =>
		useDouble?	new Constant(Math.Atan(c1.doubleValue)):
					new Constant(factory.Atan(c1.decimalValue));
}