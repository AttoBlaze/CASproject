using Commands;
using Application;

namespace CAS;

public interface EqualityComparer<T> {
    public bool Equals(T obj);
}

public interface Simplifiable<T> {
    public T Simplify();
}

public interface Differentiable<T> {
    public T Differentiate(string variable) => throw new Exception("Expression is not differentiable");
}

public interface Evaluatable<T> {
    public T Evaluate() => Evaluate(new());
    public T Evaluate(Dictionary<string,MathObject> definedObjects);
}

/// <summary>
/// Represents a mathematical object/expression
/// </summary>
public interface MathObject : EqualityComparer<MathObject>, Simplifiable<MathObject>, Evaluatable<MathObject>, Differentiable<MathObject> {
    /// <summary>
	/// Parses a string input into a math object 
	/// </summary>
	public static MathObject Parse(string input) => (MathObject)Program.ParseInput(input);

    public MathObject Diff(string variable) => this.Calculate().Differentiate(variable).Simplify();
    public MathObject Calculate() => Calculate(Program.definedObjects);
    public MathObject Calculate(Dictionary<string,MathObject> definedObjects) => 
        this.Evaluate(definedObjects).Simplify();

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