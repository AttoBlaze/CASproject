using CAS;
using Application;

namespace Algebra;

public class DerivativeTests {
	public MathObject Parse(string input) => MathObject.Parse(input);
	public MathObject ParseDiff(string input,string variable) => MathObject.Parse(input).Diff(variable);

	[SetUp]
	public void setup() => Program.START();

    [Test]
    public void Single() {
		Assert.That(ParseDiff("x","x").Equals((Constant)1));
		Assert.That(ParseDiff("x","y").Equals((Constant)0));
	}

	[Test]
	public void Sum() {
		Assert.That(ParseDiff("x+x","x").Equals(Parse("2")));
		Assert.That(ParseDiff("x^2+x","x").Equals(Parse("2x+1")));
	}

	[Test]
	public void Product() {
		Assert.That(Parse("x*x").Differentiate("x").Simplify().Equals(Parse("2x")));
		Assert.That(ParseDiff("x*ln(x)","x").Equals(Parse("ln(x)+1")));
		Assert.That(ParseDiff("2*ln(x)","x").Equals(Parse("2/x")));
	}

	[Test]
	public void Power() {
		Assert.That(ParseDiff("x^2","x").Equals(Parse("2x")));
		Assert.That(Parse("e^x").Differentiate("x").Simplify().Equals(Parse("e^x")));
		Assert.That(ParseDiff("ln(x)^ln(x)","x").Equals(Parse("ln(x)^ln(x)*(1+ln(ln(x)))/x")));
	}

	[Test]
	public void Ln() {
		Assert.That(ParseDiff("ln(x)","x").Equals(Parse("1/x")));
	}
}