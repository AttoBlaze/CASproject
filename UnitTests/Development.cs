using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "define(y;x^2*(x*x)^2*x+ln(x)^ln(x)*x^3+x)"
        );
        Program.Execute("write("+string.Join("+",Enumerable.Range(0,10).Select(n => "time(Diff(y;x))").ToArray())+")");
        Assert.Pass();
    }
}