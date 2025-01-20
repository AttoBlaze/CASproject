using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "time(Diff(x^3;x))",
            "time(explain(time))"
        );
        Assert.Pass();
    }
}