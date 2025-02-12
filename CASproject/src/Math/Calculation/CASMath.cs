using DecimalSharp;
using Application;

namespace CAS;

public static partial class CASMath {
	/// <summary>
	/// Wether or not calculations are made using double by default.
	/// </summary>
	public static bool UseDouble = true;

	/// <summary>
	/// The precision of calculations when calculating with arbitrary precision.
	/// </summary>
	public static long Precision {get => factory.Config.Precision; set {
		factory.Config.Precision = Math.Clamp(value,1,1000);
		if(value>16) {
			Program.Predefine("e",E);
			Program.Predefine("pi",PI);
		}
	}}
	public readonly static BigDecimalFactory factory = new BigDecimalFactory(new BigDecimalConfig(){Precision = 40});

	/// <summary>
	/// The mathematical constant e
	/// </summary>
	public static Constant E {get => Precision<16? Math.E:factory.Exp(1);}
	
	/// <summary>
	/// The mathematical constant pi
	/// </summary>
	public static Constant PI {get => Precision<16? Math.PI:factory.Asin(0);}
}