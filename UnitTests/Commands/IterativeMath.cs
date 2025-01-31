namespace CommandTests;

using CAS;
using Commands;
using Application;

public class IterativeMath {
	[SetUp] public void setup() => Program.START();

	[Test]
	public void Recursion() {
		Program.Execute(
			"define(fibonacci;k;recurse((n;m);(0;1);k;n+m))",
            "fibonacci(10)",
            "define(f;x;recurse(i;1;x;i+1))",
            "f(10)"
		);
		Assert.That(MathObject.Parse("fibonacci(9)").Calculate().AsValue()==55);
	}

	[Test]
	public void Product() {
		Program.Execute(
			"define(factorial;k;product(n;1;k;n))",
            "factorial(10)",
            "define(f;x;product(i;1;x;i^2))",
            "f(10)"
		);
		Assert.That(MathObject.Parse("factorial(5)").Calculate().AsValue()==120);
	}
}