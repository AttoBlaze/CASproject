namespace CAS;

public partial class CASMath{
	public static Constant Add(Constant c1, Constant c2) => Calculator.add(c1,c2);
	public Constant add(Constant c1, Constant c2) =>
		arbitraryPrecision?	
			new Constant(factory.Add(c1.decimalValue,c2.decimalValue)):
			new Constant(c1.doubleValue+c2.doubleValue);
	
	public static Constant Subtract(Constant c1, Constant c2) => Calculator.subtract(c1,c2);
	public Constant subtract(Constant c1, Constant c2) =>
		arbitraryPrecision?	
			new Constant(factory.Sub(c1.decimalValue,c2.decimalValue)):
			new Constant(c1.doubleValue-c2.doubleValue);
		
	public static Constant Multiply(Constant c1, Constant c2) => Calculator.multiply(c1,c2);
	public Constant multiply(Constant c1, Constant c2) =>
		arbitraryPrecision? 	
			new Constant(factory.Mul(c1.decimalValue,c2.decimalValue)):
			new Constant(c1.doubleValue*c2.doubleValue);
		
	public static Constant Divide(Constant c1, Constant c2) => Calculator.divide(c1,c2);
	public Constant divide(Constant c1, Constant c2) =>
		arbitraryPrecision? 	
			new Constant(factory.Div(c1.decimalValue,c2.decimalValue)):
			new Constant(c1.doubleValue/c2.doubleValue);

	public static Constant Pow(Constant c1, Constant c2) => Calculator.pow(c1,c2);
	public Constant pow(Constant c1, Constant c2) => 
		arbitraryPrecision?	
			new Constant(factory.Pow(c1.decimalValue,c2.decimalValue)):
			new Constant(Math.Pow(c1.doubleValue,c2.doubleValue));
	
}