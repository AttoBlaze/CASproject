namespace Algebra;

using CAS;
using Commands;
using Application;

public class Simplifications {
	private static MathObject Parse(string input) => MathObject.Parse(input);
	private static MathObject ParseSimplify(string input) => MathObject.Parse(input).Simplify();
	private static double ParseSimplifyVal(string input) => ParseSimplify(input).AsValue(); 

	[Test]
	public void Multiply() {
		Assert.That(ParseSimplifyVal("1*2")==2);
		Assert.That(ParseSimplify("1*a").Equals(Parse("a")));
		Assert.That(ParseSimplify("a*a").Equals(Parse("a^2")));
		Assert.That(ParseSimplify("a*a^n").Equals(Parse("a^(n+1)")));
		Assert.That(ParseSimplify("a^b*a^c").Equals(Parse("a^(b+c)")));
		Assert.That(ParseSimplify("1*a*a^b*a^c").Equals(Parse("a^(b+c+1)")));
	}

	[Test]
	public void Add() {
		Assert.That(ParseSimplifyVal("1+2")==3);
		Assert.That(ParseSimplify("0+a").Equals(Parse("a")));
		Assert.That(ParseSimplify("a+a").Equals(ParseSimplify("2*a")));
		Assert.That(ParseSimplify("a+2*a").Equals(Parse("3*a")));
		Assert.That(ParseSimplify("2*a+2*a").Equals(Parse("4*a")));
	}
}