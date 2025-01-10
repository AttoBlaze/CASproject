using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute("define(f;x;1)");
        Program.Execute("define(f;x;f(x)+1)");
        Assert.Pass();
    }
}