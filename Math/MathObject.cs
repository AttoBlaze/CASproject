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
    public T Evaluate(Dictionary<string,double> definedVariables);
}

/// <summary>
/// Represents a mathematical object/expression
/// </summary>
public interface MathObject : EqualityComparer<MathObject>, EquivalenceComparer<MathObject>, Simplifiable<MathObject>, Evaluatable<MathObject> {
        public MathObject Calculate(Dictionary<string,double> definedVariables) => 
            this.Evaluate(definedVariables).Simplify();

    public string AsString();
}