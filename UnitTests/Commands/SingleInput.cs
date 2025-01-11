using Application;

namespace CommandTests;

public class SingleInputTests {
    [Test]
    public void ListObjects() {
        Program.START();
        Program.ExecuteAll(Commands.ListObjects.listables.Keys.Select(n => "list("+n+")"));
        Assert.Pass();
    }
}