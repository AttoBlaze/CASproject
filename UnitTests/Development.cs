using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "define(x1;1)",
			"define(x2;x;x+1)",
			"x1+1",
			"x2(10)"
        );
		Assert.Pass();
    }
}