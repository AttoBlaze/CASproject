using Commands;
using CAS;
using Application;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
            "define(fibonacci;k;recurse((n;m);(0;1);k;n+m))",
            "fibonacci(10)",
            "define(f;x;recurse(i;1;x;i+1))",
            "f(10)",
            "recurse(i;0;1000;i+Time(Diff(tan(ln(x)/x)/atan(x);x)))"
        );
        Assert.Pass();
    }
}