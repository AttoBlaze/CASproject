using Commands;
using Application;

namespace CAS;

public interface EqualityComparer<T> {
    public bool Equals(T obj);
}

public interface Simplifiable<T1,T2> {
    public T2 Simplify(T1 input);
}

public interface Differentiable<T1,T2> {
    public T2 Differentiate(string variable,T1 input) => throw new Exception("Expression is not differentiable");
}

public interface Evaluatable<T> {
    public T Evaluate() => Evaluate(new());
    public T Evaluate(Dictionary<string,MathObject> definedObjects);
}

public struct SimplificationSettings {
	/// <summary>
	/// Whether or not constants are calculated on simplification (eg. if 4/2 is simplified to 2)
	/// </summary>
	public bool calculateConstants;

	/// <summary>
	/// If a variable called e is presumed to be eulers number.
	/// </summary>
	public bool eIsEulersNumber;
	/// <summary>
	/// If parentheses like (a+b)*(c+d) is expanded to ac+ad+bc+bd.
	/// </summary>
	public bool expandParentheses;
	public CASMath calculator;
	public static SimplificationSettings Calculation = new(){
		calculateConstants = true, 
		eIsEulersNumber = true,
		expandParentheses = false,
		calculator = Program.Calculator
	};
}

public struct CalculusSettings {
	public bool eIsEulersNumber;
	public static CalculusSettings Calculation = new(){
		eIsEulersNumber = true
	};
}

/// <summary>
/// Represents a mathematical object/expression
/// </summary>
public interface MathObject : EqualityComparer<MathObject>, Simplifiable<SimplificationSettings,MathObject>, Evaluatable<MathObject>, Differentiable<CalculusSettings,MathObject> {

	/// <summary>
	/// Parses a string input into a math object 
	/// </summary>
	public static MathObject Parse(string input) => (MathObject)Program.ParseInput(input);


	public MathObject Simplify() => Simplify(Program.simplificationSettings);
	public MathObject Differentiate(string variable) => Differentiate(variable,Program.calculusSettings);
	public MathObject Diff(string variable) => this.Calculate().Differentiate(variable,Program.calculusSettings).Simplify();
    
	/// <summary>
	/// Evaluates & simplifies this math object using the objects defined in the program
	/// </summary>
	public MathObject Calculate() => Calculate(Program.definedObjects,Program.simplificationSettings);
    /// <summary>
	/// Evaluates & simplifies this math object
	/// </summary>
	public MathObject Calculate(Dictionary<string,MathObject> definedObjects, SimplificationSettings simplificationSettings) => 
        this.Evaluate(definedObjects).Simplify(simplificationSettings);

    /// <summary>
    /// Gives a string representation of this math object. 
    /// </summary>
    public string AsString();

    /// <summary>
    /// Wether or not the definition of this math object contains the given term 
    /// </summary>
    public bool ContainsAny(MathObject term) => Equals(term);
    
    /// <summary>
    /// Gets the value of this math object as a double.
    /// </summary>
    /// <exception cref="Exception">If this object is not a value</exception>
    public double AsValue() => throw new Exception("Not a value!");

	/// <summary>
	/// The precedence of this term. Mainly used to make string parentheses.
	/// </summary>
	public int Precedence() => 0;
    public int AbsPrecedence() => Math.Abs(Precedence()); 
    
    /// <summary>
    /// Casts this to the given type. Only used to remove nested parentheses. 
    /// </summary>
    public T As<T>() => (T)this;

    /// <summary>
    /// Attempts to find another term in the given list which upholds the predicate condition <br/>
    /// If one is found, the term is removed from the list.
    /// </summary>
    public static bool FindAndRemoveOtherTerm(Func<MathObject,bool> predicate, List<MathObject> terms, ref int index, ref MathObject otherTerm, ref int otherIndex) {
        if(FindOtherTerm(predicate,terms,index,ref otherTerm,ref otherIndex)) {
            terms.RemoveAt(otherIndex);
            if (otherIndex<index) index--;
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Attempts to find another term in the given list which upholds the predicate condition
    /// </summary>
    public static bool FindOtherTerm(Func<MathObject,bool> predicate, List<MathObject> terms, int index, ref MathObject otherTerm, ref int otherIndex) {
        for(int i=0; i<terms.Count ; i++) {
            if(i==index) continue;
            if(predicate(terms[i])) {
                otherTerm = terms[i];
                otherIndex = i;
                return true;
            }
        }
        return false;
    }
}

/// <summary>
/// Specifies that this object contains a name. Mainly used to convert math objects to string inputs.
/// </summary>
public interface NamedObject {
    public string GetName();
}

public static class MathObjectExtensions {
    public static bool FindOtherTerm(this List<MathObject> terms, Func<MathObject,bool> predicate, out int otherIndex) {
        for(int i=0; i<terms.Count ; i++) {
            if(predicate(terms[i])) {
                otherIndex = i;
                return true;
            }
        }
        otherIndex = default;
        return false;
    } 
}