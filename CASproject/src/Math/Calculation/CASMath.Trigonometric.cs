namespace CAS;

public partial class CASMath {
	public static Constant Sin(Constant c1) => Calculator.sin(c1);
	public Constant sin(Constant c1) =>
		arbitraryPrecision?	
			new Constant(factory.Sin(c1.decimalValue)):
			new Constant(Math.Sin(c1.doubleValue));

	public static Constant Cos(Constant c1) => Calculator.cos(c1);
	public Constant cos(Constant c1) =>
		arbitraryPrecision?	
			new Constant(factory.Cos(c1.decimalValue)):
			new Constant(Math.Cos(c1.doubleValue));

	public static Constant Tan(Constant c1) => Calculator.tan(c1);
	public Constant tan(Constant c1) =>
		arbitraryPrecision?	
			new Constant(factory.Tan(c1.decimalValue)):
			new Constant(Math.Tan(c1.doubleValue));
	
	public static Constant Asin(Constant c1) => Calculator.asin(c1);
	public Constant asin(Constant c1) =>
		arbitraryPrecision?	
			new Constant(factory.Asin(c1.decimalValue)):
			new Constant(Math.Asin(c1.doubleValue));

	public static Constant Acos(Constant c1) => Calculator.acos(c1);
	public Constant acos(Constant c1) =>
		arbitraryPrecision?	
			new Constant(factory.Acos(c1.decimalValue)):
			new Constant(Math.Acos(c1.doubleValue));

	public static Constant Atan(Constant c1) => Calculator.atan(c1);
	public Constant atan(Constant c1) =>
		arbitraryPrecision?	
			new Constant(factory.Atan(c1.decimalValue)):
			new Constant(Math.Atan(c1.doubleValue));
}