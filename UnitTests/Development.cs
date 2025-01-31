using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "product(k;1;5;k)"
        );
        Assert.Pass();
    }
}