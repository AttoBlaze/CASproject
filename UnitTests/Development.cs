using Commands;
using CAS;
using Application;
using DecimalSharp;
using DecimalSharp.Core;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
		Program.START();
        Program.Execute(
			"pi"
        );
		Assert.Pass();
    }
}