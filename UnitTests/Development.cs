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
			"(x+x)^2"
			);
		Assert.Pass();
}
}