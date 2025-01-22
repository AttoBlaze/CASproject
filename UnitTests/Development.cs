using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "explain(help)",
            "define(f;x;y;x^y)",
            "f(2;3)"
        );
        Assert.Pass();
    }
}