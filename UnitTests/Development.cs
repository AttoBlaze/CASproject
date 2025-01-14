using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "define(f;x;x+1)",
            "list(functions)",
            "f(y)",
            "f"
        );
        Assert.Pass();
    }
}