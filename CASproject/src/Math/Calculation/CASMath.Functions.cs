namespace CAS;

public partial class CASMath {
	public static Constant Log(Constant c1) => Calculator.log(c1);
	public Constant log(Constant c1) => 
		arbitraryPrecision?	
			new Constant(factory.Ln(c1.decimalValue)):
			new Constant(Math.Log(c1.doubleValue));
	
	public static Constant Exp(Constant c1) => Calculator.exp(c1);
	public Constant exp(Constant c1) =>
		arbitraryPrecision?	
			new Constant(factory.Exp(c1.doubleValue)):
			new Constant(Math.Exp(c1.doubleValue));
}