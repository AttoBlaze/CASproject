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

public interface Calculateable<T> {
    public T Calculate() => Calculate(new());
    public T Calculate(Dictionary<string,double> definedVariables);
}

public interface MathObject : EqualityComparer<MathObject>, EquivalenceComparer<MathObject>, Simplifiable<MathObject>, Calculateable<MathObject> {
    public string AsString();
}