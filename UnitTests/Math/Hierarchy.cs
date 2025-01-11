using CAS;

namespace Algebra;

public class HierarchyTests {
    private static double eval(string input) => MathObject.Parse(input).Calculate().AsValue();

    [Test]
    public void ParseExpression() {
        Assert.That(
            eval("11+7*5/3^2"),
            Is.EqualTo(11+7*5/Math.Pow(3,2))
        );
    }

    [Test] 
    public void Sum() {
        Assert.That(eval("2 + 2+3"),Is.EqualTo(2+2+3));
        Assert.That(eval("2 + 2-3"),Is.EqualTo(2+2-3));
        Assert.That(eval("2 + 2*3"),Is.EqualTo(2+2*3));
        Assert.That(eval("2 + 2/3"),Is.EqualTo(2+(double)2/3));
        Assert.That(eval("2 + 2^3"),Is.EqualTo(2+Math.Pow(2,3)));
    }

    [Test] 
    public void Product() {
        Assert.That(eval("2 * 2+3"),Is.EqualTo(2*2+3));
        Assert.That(eval("2 * 2-3"),Is.EqualTo(2*2-3));
        Assert.That(eval("2 * 2*3"),Is.EqualTo(2*2*3));
        Assert.That(eval("2 * 2/3"),Is.EqualTo(2*(double)2/3));
        Assert.That(eval("2 * 2^3"),Is.EqualTo(2*Math.Pow(2,3)));
    }

    [Test] 
    public void Divide() {
        Assert.That(eval("2 / 2+3"),Is.EqualTo(2/2+3));
        Assert.That(eval("2 / 2-3"),Is.EqualTo(2/2-3));
        Assert.That(eval("2 / 2*3"),Is.EqualTo(2/2*3));
        Assert.That(eval("2 / 2/3"),Is.EqualTo(2/(double)2/3));
        Assert.That(eval("2 / 2^3"),Is.EqualTo(2/Math.Pow(2,3)));
    }

    [Test] 
    public void Power() {
        Assert.That(eval("2 ^ 2+3"),Is.EqualTo(Math.Pow(2,2)+3));
        Assert.That(eval("2 ^ 2-3"),Is.EqualTo(Math.Pow(2,2)-3));
        Assert.That(eval("2 ^ 2*3"),Is.EqualTo(Math.Pow(2,2)*3));
        Assert.That(eval("2 ^ 2/3"),Is.EqualTo(Math.Pow(2,2)/3));
        Assert.That(eval("2 ^ 2^3"),Is.EqualTo(Math.Pow(2,Math.Pow(2,3))));
    }
}