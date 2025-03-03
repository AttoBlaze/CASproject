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
	#region Shortcut methods
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
	#endregion
	
	#region Interface methods 
    /// <summary>
    /// Gives a string representation of this math object. 
    /// </summary>
    public string AsString();

    /// <summary>
    /// Wether or not the definition of this math object contains the given term 
    /// </summary>
    public bool ContainsAny(MathObject term) => this.Equals(term);

	/// <summary>
	/// The precedence of this term. Mainly used to make string parentheses.
	/// </summary>
	public int Precedence() => 0;
    public int AbsPrecedence() => Math.Abs(Precedence()); 
    #endregion

	#region Utility
    /// <summary>
    /// Gets the value of this math object as a double.
    /// </summary>
    /// <exception cref="Exception">If this object is not a value</exception>
    public double AsValue() => throw new Exception("Not a value!");
    
	/// <summary>
    /// Casts this to the given type. Only used to remove nested parentheses. 
    /// </summary>
    public T As<T>() => (T)this;

    /// <summary>
    /// Attempts to find another term in the given list which upholds the predicate condition <br/>
    /// If one is found, the term is removed from the list.
    /// </summary>
    public static bool FindAndRemoveOtherTerm(Predicate<MathObject> predicate, List<MathObject> terms, ref int index, ref MathObject otherTerm, ref int otherIndex) {
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
    public static bool FindOtherTerm(Predicate<MathObject> predicate, List<MathObject> terms, int index, ref MathObject otherTerm, ref int otherIndex) {
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


	/// <summary>
	/// Checks if the given lists contain the exact same elements in the same order.
	/// </summary>
	public static bool TermListsAreExactlyEqual(IEnumerable<MathObject> list1, IEnumerable<MathObject> list2) {
		//count check
		if(list1.Count()!=list2.Count()) return false;
		
		//enumerate and check each term
		IEnumerator<MathObject> enumerator1 = list1.GetEnumerator(), enumerator2 = list2.GetEnumerator(); 
		while(enumerator1.MoveNext()) {
			enumerator2.MoveNext();
			if(!enumerator1.Current.Equals(enumerator2.Current)) return false;
		}
		return true;
	}

	/// <summary>
	/// Checks if the given lists contain exactly the same elements regardless of order. <br/>
	/// NOTE: This method has poor time complexity. If two lists are known to be in a deterministic/predictable order, it is recommended to use <see href="TermListsAreExactlyEqual">TermListsAreExactlyEqual</see> .
	/// </summary>
	public static bool TermListsContainSameElements(IEnumerable<MathObject> list1, IEnumerable<MathObject> list2) {
		//count check
		if(list1.Count()!=list2.Count()) return false;
		
		//enumerate and check each term
		var list2Copy = list2.ToList();
		foreach(var term in list1) {
			//remove the term if found
			if(list2Copy.FindOtherTerm(n => n.Equals(term),out int index)) list2Copy.RemoveAt(index);
			
			//if it is not found then the lists dont contain the same elements
			else return false;
		}
		return true;
	}
}

/// <summary>
/// Specifies that this object uses math objects 
/// </summary>
public interface MathObjectHolder {
	public IEnumerable<MathObject> GetMathArgumentList();
}

/// <summary>
/// Specifies that this object contains a name. Mainly used to convert math objects to string inputs.
/// </summary>
public interface NamedObject {
    public string GetName();
}

public static class MathObjectExtensions {
    /// <summary>
	/// Attempts to find a term which satifies the given condition
	/// </summary>
	/// <returns> true if a term satisfying the given condition is found. The otherIndex will here be set to the index of the term found.</returns>
	public static bool FindOtherTerm(this List<MathObject> terms, Predicate<MathObject> predicate, out int otherIndex) {
        if(terms.Count==0) {
			otherIndex=default;
			return false;
		}
 		otherIndex = terms.FindIndex(predicate);
		return otherIndex!=-1;
    }

	//public static bool ContainsSameTermsAs(this List<MathObject> terms, List<MathObject> other) => MathObject.TermListsContainSameElements(terms,other);
	//public static bool IsExactlyEqualTo(this List<MathObject> terms, List<MathObject> other) => MathObject.TermListsAreExactlyEqual(terms,other);
}
#endregion