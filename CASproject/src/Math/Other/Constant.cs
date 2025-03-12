using DecimalSharp;
using Application;

namespace CAS;

/// <summary>
/// Represents a constant value
/// </summary>
public class Constant : MathObject { 
	//double precision
	public double doubleValue {
		get => _doubleValue; 
		set {
			_doubleValue = value;
			_decimalValue = null; //deinit arbitrary precision
		}
	}
	private double _doubleValue;
	
	//arbitrary precision- prevent initializing until usage is needed when calculating with double precision for performance
	public BigDecimal decimalValue {
		get{
			_decimalValue ??= new BigDecimal(doubleValue);	//delayed init if value was defined from a double val
			return _decimalValue;
		} 
		set {
			_decimalValue = value;
			_doubleValue = value.ToNumber();
		}
	}
	private BigDecimal? _decimalValue = null;

	private Constant(double val, BigDecimal big) {
		this._decimalValue = big;
		this._doubleValue = val;
	}
	public Constant(double value) {
        this._doubleValue = value;
	}
	public Constant(BigDecimal value) {
        this._decimalValue = value;
		this._doubleValue = value.ToNumber();
	}
	
	public Constant(Constant constant) {
		this._decimalValue = constant._decimalValue;
		this._doubleValue = constant._doubleValue;
	}

	public Constant(string str) {
		this._decimalValue = new BigDecimal(str.Replace(',','.'));
		this._doubleValue = double.Parse(str.Replace('.',','));
	}

	public bool IsZero => _decimalValue?.IsZero()??true && doubleValue==0;
	public bool IsOne => _decimalValue?.Equals(1)??true && doubleValue==1;
	public bool IsWhole => _decimalValue?.IsInteger()??true && doubleValue%1==0;
	public bool IsPositive => _decimalValue?.IsPositive()??true && doubleValue>0;
	public bool IsNegative => _decimalValue?.IsPositive()??true && doubleValue<0;
	public bool IsPowerOfTwo =>	_decimalValue?.Log(2).IsInteger()??false || Math.Log2(doubleValue)%1==0;

	public double AsValue() => doubleValue;

    public MathObject Evaluate(Dictionary<string, MathObject> definedObjects) {
		//constants evaluate to themselves
        return new Constant(this);
    }

    public MathObject Simplify(SimplificationSettings settings) {
        //constants cannot be simplified
        return new Constant(this);
    }
	
    public MathObject Differentiate(string variable, CalculusSettings settings) => new Constant(0d);

    public bool Equals(MathObject obj) =>
        obj is Constant c &&  //same type
		((c._decimalValue==null && this._decimalValue==null) || c.decimalValue.Equals(this.decimalValue)) &&	//same value
		c.doubleValue==doubleValue;  

	//format to prevent double strings being written with exponential notation
	private static readonly string format = StringTree.StringOf('#',50)+"0."+StringTree.StringOf('#',50);
    public string AsString() {
		string str =_decimalValue!=null && decimalValue.Precision()>CASMath.DOUBLE_PRECISION? decimalValue.ToString():doubleValue.ToString(format);
		if(str.Contains('e')) str = "("+str.Replace("e","*10^").Replace("+","")+")";
		return str.Replace(",",".");
	}

	//conversions
	public static implicit operator Constant(double val) => new(val);
	public static implicit operator Constant(BigDecimal val) => new(val);
	
	//operators
	public static Constant operator +(Constant left, Constant right) => CASMath.Add(left, right);
	public static Constant operator -(Constant left, Constant right) => CASMath.Subtract(left, right);
	public static Constant operator -(Constant c) => c.Negate();
	public static Constant operator *(Constant left, Constant right) => CASMath.Multiply(left, right);
	public static Constant operator /(Constant left, Constant right) => CASMath.Divide(left, right);
	
	
	public Constant Negate() {
		if(_decimalValue!=null) return new Constant(-doubleValue,_decimalValue.Neg());
		return new Constant(-doubleValue);
	} 
}