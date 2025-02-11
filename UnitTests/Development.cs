using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "sin(1+1)"
        );
		Assert.Pass();
    }
}