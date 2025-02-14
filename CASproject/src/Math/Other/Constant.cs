using DecimalSharp;
using Application;

namespace CAS;

/// <summary>
/// Represents a constant value
/// </summary>
public class Constant : MathObject, Differentiable<MathObject> { 
    public double doubleValue {get; private set;}
	public BigDecimal decimalValue {get; private set;}
	private Constant(double val, BigDecimal big) {
		this.doubleValue = val;
		this.decimalValue = big;
	}
	public Constant(double value) {
        this.doubleValue = value;
		this.decimalValue = new BigDecimal(value);
	}
	public Constant(BigDecimal value) {
        this.doubleValue = value.ToNumber();
		this.decimalValue = value;
	}
	
	public Constant(Constant constant) {
		this.doubleValue = constant.doubleValue;
		this.decimalValue = constant.decimalValue;
	}

	public Constant(string str) {
		this.decimalValue = CASMath.Calculator.factory.BigDecimal(str);
		this.doubleValue = CASMath.Precision>16? this.decimalValue.ToNumber():double.Parse(str);
	}

	public bool IsZero {get => decimalValue.IsZero();}
	public bool IsOne {get => decimalValue.Equals(1);}

	public double AsValue() => doubleValue;

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
		//constants evaluate to themselves
        return new Constant(this);
    }

    public MathObject Simplify(SimplificationSettings settings) {
        //constants cannot be simplified
        return new Constant(this);
    }

    public MathObject Differentiate(string variable) => new Constant(0d);

    public bool Equals(MathObject obj) =>
        obj is Constant &&  //same type
        ((Constant)obj).doubleValue==doubleValue;  //same value

	//format to prevent double strings being written with exponential notation
    private static readonly string format = StringTree.StringOf('#',300)+"0."+StringTree.StringOf('#',300);
    public string AsString() => decimalValue.Precision()>16? decimalValue.ToString():doubleValue.ToString(format);

	//conversions
	public static implicit operator Constant(double val) => new(val);
	public static implicit operator double(Constant val) => val.doubleValue;
	public static implicit operator Constant(BigDecimal val) => new(val);
	public static implicit operator BigDecimal(Constant val) => val.decimalValue;
	
	
	//operators
	public static Constant operator +(Constant left, Constant right) => CASMath.Add(left, right);
	public static Constant operator -(Constant left, Constant right) => CASMath.Subtract(left, right);
	public static Constant operator -(Constant c) => c.Negate();
	public static Constant operator *(Constant left, Constant right) => CASMath.Multiply(left, right);
	public static Constant operator /(Constant left, Constant right) => CASMath.Divide(left, right);
	public Constant Negate() => new Constant(-doubleValue,decimalValue.Neg());
}