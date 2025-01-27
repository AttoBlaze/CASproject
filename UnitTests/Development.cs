using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "define(V;t;p;(n*R*t)/p)"
        );
        Assert.Pass();
    }
}