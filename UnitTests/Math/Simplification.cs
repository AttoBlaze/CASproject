namespace Algebra;

using CAS;
using Commands;
using Application;

public class Simplifications {
	private static MathObject Parse(string input) => MathObject.Parse(input);
	private static MathObject ParseSimplify(string input) => MathObject.Parse(input).Simplify();
	private static double ParseSimplifyVal(string input) => ParseSimplify(input).AsValue(); 

	[SetUp] public void startup() => Program.START();

	[Test]
	public void Multiply() {
		Assert.That(ParseSimplifyVal("1*2")==2);
		Assert.That(ParseSimplify("1*a").Equals(Parse("a")));
		Assert.That(ParseSimplify("a*a").Equals(Parse("a^2")));
		Assert.That(ParseSimplify("a*a*a").Equals(Parse("a^3")));
		Assert.That(ParseSimplify("a*a^n").Equals(Parse("a^(n+1)")));
		Assert.That(ParseSimplify("a^b*a^c").Equals(Parse("a^(b+c)")));
		Assert.That(ParseSimplify("1*a*a^b*a^c").Equals(Parse("a^(b+c+1)")));
		Assert.That(ParseSimplifyVal("0*a")==0);
	}

	[Test]
	public void Add() {
		Assert.That(ParseSimplifyVal("1+2")==3);
		Assert.That(ParseSimplify("0+a").Equals(Parse("a")));
		Assert.That(ParseSimplify("a+a").Equals(ParseSimplify("2*a")));
		Assert.That(ParseSimplify("a+2*a").Equals(Parse("3*a")));
		Assert.That(ParseSimplify("2*a+2*a").Equals(Parse("4*a")));
		Program.Log(ParseSimplify("a-a").AsString());
		Assert.That(ParseSimplifyVal("a-a")==0);
		Assert.That(ParseSimplifyVal("2*a-2*a")==0);
		Assert.That(ParseSimplifyVal("3*a-2*a-a")==0);
	}

	[Test]
	public void Ln() {
		Program.START();
		Assert.That(ParseSimplify("ln(1)").Equals(ParseSimplify("0")));
		Assert.That(ParseSimplify("ln(e)").Equals(ParseSimplify("1")));
		Assert.That(ParseSimplify("ln(e^x)").Equals(ParseSimplify("x")));
	}

	[Test]
	public void Power() {
		Assert.That(ParseSimplifyVal("2^3")==8);
		Assert.That(ParseSimplifyVal("a^0")==1);
		Assert.That(ParseSimplify("a^1").Equals(ParseSimplify("a")));
	}

	[Test] 
	public void Sin() {
		Assert.That(ParseSimplifyVal("sin(0)")==0);
		Assert.That(ParseSimplifyVal("asin(0)")==0);
		Assert.That(ParseSimplify("sin(asin(x))").Equals(Parse("x")));
		Assert.That(ParseSimplify("asin(sin(x))").Equals(Parse("x")));
	}

	[Test] 
	public void Cos() {
		Assert.That(ParseSimplifyVal("cos(0)")==1);
		Assert.That(ParseSimplifyVal("acos(1)")==0);
		Assert.That(ParseSimplify("cos(acos(x))").Equals(Parse("x")));
		Assert.That(ParseSimplify("acos(cos(x))").Equals(Parse("x")));
	}

	[Test] 
	public void Tan() {
		Assert.That(ParseSimplifyVal("tan(0)")==0);
		Assert.That(ParseSimplifyVal("atan(0)")==0);
		Assert.That(ParseSimplify("tan(atan(x))").Equals(Parse("x")));
		Assert.That(ParseSimplify("atan(tan(x))").Equals(Parse("x")));
	}
}