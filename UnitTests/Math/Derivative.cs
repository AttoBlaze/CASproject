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

	[Test] public void Ln() => Assert.That(ParseDiff("ln(x)","x").Equals(Parse("1/x")));
	[Test] public void Sin() => Assert.That(ParseDiff("sin(x)","x").Equals(Parse("cos(x)")));
	[Test] public void Asin() => Assert.That(ParseDiff("asin(x)","x").Equals(Parse("1/(-x^2+1)^0.5")));
	[Test] public void Cos() => Assert.That(ParseDiff("cos(x)","x").Equals(Parse("-sin(x)")));
	[Test] public void Acos() => Assert.That(ParseDiff("acos(x)","x").Equals(Parse("-1/(-x^2+1)^0.5").Simplify()));
	[Test] public void Tan() => Assert.That(ParseDiff("tan(x)","x").Equals(Parse("tan(x)^2+1")));
	[Test] public void Atan() => Assert.That(ParseDiff("atan(x)","x").Equals(Parse("1/(x^2+1)").Simplify()));
	
	
}