using System.Numerics;

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
    public MathObject Calculate() => Calculate(new());
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
    public int Precedence() => 0;
    public int AbsPrecedence() => Math.Abs(Precedence()); 
    public double AsValue() => throw new Exception("Not a value!");
}

public interface NamedObject {
    public string GetName();
}