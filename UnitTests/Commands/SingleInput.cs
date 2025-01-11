using Application;

namespace CommandTests;

public class SingleInputTests {
    [Test]
    public void ListObjects() {
        Program.START();
        Program.ExecuteAll(Commands.ListObjects.listables.Keys.Select(n => "list("+n+")"));
        Assert.Pass();
    }

    [Test]
    public void Help() {
        Program.START();
        Program.Execute("help()");
        Assert.Pass();
    }

    [Test]
    public void Explain() {
        Program.START();
        Program.Execute("explain(help)");
        Program.Execute("explain(AlwaysWrite)");
        Assert.Pass();
    }
}