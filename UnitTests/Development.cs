using Commands;
using CAS;
using Application;
using DecimalSharp;
using DecimalSharp.Core;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
		Program.Execute(
			"pi"
        );
		Console.WriteLine(new BigDecimal("0.14").ToString());
		Console.WriteLine(double.Parse("0,14").ToString());
		Assert.Pass();
}
}