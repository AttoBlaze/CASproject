using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "write(Simplify(x*x))"
        );
        Assert.Pass();
    }
}