using Commands;
using CAS;

namespace Algebra;

public class ComparisonTests {
    [Test]
    public void Contains() {
        var expr = MathObject.Parse("x+231*(z+2^4*y*(10+2*(2+1^c)))");
        Assert.That(expr.Contains(new Variable("x")));
        Assert.That(expr.Contains(new Variable("y")));
        Assert.That(expr.Contains(new Variable("z")));
        Assert.That(expr.Contains(new Variable("c")));
    }

    /*[Test]
    public void Equals() {
        Assert.Inconclusive();
    }*/
}