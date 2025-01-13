using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "define(f;x;x^2)",
            "f(2)",
            "f(x)"
        );
        Assert.Pass();
    }
}