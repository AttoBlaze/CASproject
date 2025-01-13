using Commands;
using Application;

namespace CAS;

public interface EqualityComparer<T> {
    public bool Equals(T obj);
}

public interface EquivalenceComparer<T> {
    public bool EquivalentTo(T obj);
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
public interface MathObject : EqualityComparer<MathObject>, EquivalenceComparer<MathObject>, Simplifiable<MathObject>, Evaluatable<MathObject> {
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

    public static bool FindOtherTerm(Func<MathObject,bool> predicate, List<MathObject> terms, int index, ref MathObject otherTerm, ref int otherIndex) {
        for(int i=0; i<terms.Count && i!=index ; i++) {
            if(predicate(terms[i])) {
                otherTerm = terms[i];
                otherIndex = i;
                return true;
            }
        }
        return false;
    }
    
    public static bool FindOtherEqualTerm(List<MathObject> terms, MathObject term, int index, ref MathObject otherTerm, ref int otherIndex) =>
        FindOtherTerm(obj=>obj.Equals(term), terms,index,ref otherTerm, ref otherIndex);
    public struct TermInfo {
        public int index;
        public MathObject obj;
        public TermInfo(MathObject obj, int index) {
            this.obj = obj;
            this.index = index;
        }
    }
}

public interface NamedObject {
    public string GetName();
}