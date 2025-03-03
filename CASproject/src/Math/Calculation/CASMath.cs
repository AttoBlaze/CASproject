using DecimalSharp;
using Application;

namespace CAS;

public partial class CASMath {
	public static CASMath Calculator = new();
	public CASMath(long precision = 40, bool arbitraryPrecision = true) {
		this.factory = new();
		factory.Config.ToExpPos = 50;
		this.arbitraryPrecision = arbitraryPrecision;
		this.precision = precision;
	}

	/// <summary>
	/// The factory used to perform calculations.
	/// </summary>
	public readonly BigDecimalFactory factory = new(new());

	/// <summary>
	/// Wether or not calculations are made using double.
	/// </summary>
	public bool arbitraryPrecision = true;
	/// <summary>
	/// Wether or not calculations are made using double on static methods.
	/// Wrapper for CASMath.Calculator.arbitraryPrecision.
	/// </summary>
	public static bool ArbitraryPrecision {get => Calculator.arbitraryPrecision; set => Calculator.arbitraryPrecision = value;}

	/// <summary>
	/// The precision of calculations when calculating with arbitrary precision.
	/// </summary>
	public long precision {
		get => factory.Config.Precision; 
		set {
			factory.Config.Precision = Math.Clamp(value,5,1000);
			
			//redefine e & pi if needed
			if(factory.Config.Precision>16) {
				_e = factory.Exp(1);
				_pi = factory.Acos(-1);
			}
		}
	}
	/// <summary>
	/// Wether or not calculations are made using double on static methods. <br/>
	/// Wrapper for CASMath.Calculator.precision.
	/// </summary>
	public static long Precision {get => Calculator.precision; set => Calculator.precision = value;}


	/// <summary>
	/// The mathematical constant e.
	/// </summary>
	public Constant e {get => new Constant(_e);}
	private Constant _e = Math.E;
	/// <summary>
	/// The mathematical constant e. <br>
	/// Wrapper for CASMath.Calculator.e
	/// </summary>
	public static Constant E {get => Calculator.e;}
	

	/// <summary>
	/// The mathematical constant pi.
	/// </summary>
	public Constant pi {get => new Constant(_pi);}
	private Constant _pi = Math.PI;
	/// <summary>
	/// The mathematical constant pi. <br>
	/// Wrapper for CASMath.Calculator.pi
	/// </summary>
	public static Constant PI {get => Calculator.pi;}
	
}