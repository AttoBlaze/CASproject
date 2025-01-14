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
    public T Differentiate(string variable);
}

public interface Evaluatable<T> {
    public T Evaluate() => Evaluate(new());
    public T Evaluate(Dictionary<string,MathObject> definedObjects);
}

/// <summary>
/// Represents a mathematical object/expression
/// </summary>
public interface MathObject : EqualityComparer<MathObject>, Simplifiable<MathObject>, Evaluatable<MathObject> {
    /// <summary>
	/// Parses a string input into a math object 
	/// </summary>
	public static MathObject Parse(string input) => (MathObject)Command.ParseInput(input);

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
    public bool Contains(MathObject term) => Equals(term);

    public T As<T>() => (T)this;
    
    public int Precedence() => 0;
    public int AbsPrecedence() => Math.Abs(Precedence()); 
    public double AsValue() => throw new Exception("Not a value!");

    public static bool FindAndRemoveOtherTerm(Func<MathObject,bool> predicate, List<MathObject> terms, ref int index, ref MathObject otherTerm, ref int otherIndex) {
        if(FindOtherTerm(predicate,terms,index,ref otherTerm,ref otherIndex)) {
            terms.RemoveAt(otherIndex);
            if (otherIndex<index) index--;
            return true;
        }
        return false;
    }
    
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