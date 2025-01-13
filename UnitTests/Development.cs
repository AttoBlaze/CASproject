using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "a^x * a * a^2"
        );
        Assert.Pass();
    }
}