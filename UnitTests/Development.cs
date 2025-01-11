using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.ExecuteAll([
            "explain(help)"
        ]);
        Assert.Pass();
    }
}