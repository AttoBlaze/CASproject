using DecimalSharp;
using Application;

namespace CAS;

/// <summary>
/// Represents a constant value
/// </summary>
public class Constant : MathObject { 
    //double precision
	public double doubleValue {get {
		if(!doubleInittet) {
			_doubleValue = _decimalValue.ToNumber();
			doubleInittet = true;
		}
		return _doubleValue;
	}}
	private double _doubleValue = 0;
	
	//arbitrary precision
	public BigDecimal decimalValue {get {
		if(!decimalInittet) {
			_decimalValue = new BigDecimal(_doubleValue);
			decimalInittet = true;
		}
		return _decimalValue;
	}}
	private BigDecimal _decimalValue = new(0);
	
	//init tracker- prevent initiating until usage is needed for performance
	private bool decimalInittet = false, doubleInittet = false;

	private Constant(double val, BigDecimal big) {
		this._decimalValue = big;
		decimalInittet = true;
		this._doubleValue = val;
		doubleInittet = true;
	}
	public Constant(double value) {
        this._doubleValue = value;
		doubleInittet = true;
	}
	public Constant(BigDecimal value) {
        this._decimalValue = value;
		this.decimalInittet = true;
	}
	
	public Constant(Constant constant) {
		this._decimalValue = constant._decimalValue;
		this.decimalInittet = constant.decimalInittet;
		this._doubleValue = constant._doubleValue;
		this.doubleInittet = constant.doubleInittet;
	}

	public Constant(string str) {
		this._decimalValue = CASMath.Calculator.factory.BigDecimal(str);
		decimalInittet = true;
		this._doubleValue = CASMath.Precision>16? this.decimalValue.ToNumber():double.Parse(str);
		doubleInittet = true;
	}

	public bool IsZero {get => decimalInittet? _decimalValue.IsZero(): _doubleValue==0;}
	public bool IsOne {get => decimalInittet? _decimalValue.Equals(1): _doubleValue==1;}

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
		(c.decimalInittet && this.decimalInittet && c._decimalValue.Equals(this._decimalValue) ||	//same value
		c.doubleValue==doubleValue);  

	//format to prevent double strings being written with exponential notation
	private static readonly string format = StringTree.StringOf('#',50)+"0."+StringTree.StringOf('#',50);
    public string AsString() {
		string str = decimalValue.Precision()>16? decimalValue.ToString():doubleValue.ToString(format);
		if(str.Contains('e')) return "("+str+")";
		return str;
	}

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
	
	
	public Constant Negate() {
		if(decimalInittet) {
			if(doubleInittet) return new Constant(-_doubleValue,_decimalValue.Neg());
			return new Constant(_decimalValue.Neg());
		}	
		return new Constant(-_doubleValue);
	} 
}