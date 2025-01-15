using Commands;
using CAS;

namespace Algebra;

public class ComparisonTests {
    [Test]
    public void Contains() {
        var expr = MathObject.Parse("x+231*(z+2^4*y*(10+2*(2+1^c)))");
        Assert.That(expr.ContainsAny(new Variable("x")));
        Assert.That(expr.ContainsAny(new Variable("y")));
        Assert.That(expr.ContainsAny(new Variable("z")));
        Assert.That(expr.ContainsAny(new Variable("c")));
    }

    /*[Test]
    public void Equals() {
        Assert.Inconclusive();
    }*/
}