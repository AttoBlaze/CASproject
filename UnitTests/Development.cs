using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "explain(Evaluate)"
        );
        Assert.Pass();
    }
}