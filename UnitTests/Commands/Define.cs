using Commands;
using CAS;
using Application;

namespace CommandsTests;

public class DefineTests{
    [Test]
    public void DefineVariableValue() {
        Program.START();
        Program.Execute("define(x;1)");
        Assert.That(Command.ParseMath("x").Calculate(Program.definedObjects).AsValue()==1);   
    }

    [Test]
    public void DefineVariableExpression() {
        Program.START();
        Program.Execute("define(x;y+1)");
        Assert.That(Command.ParseMath("x").Calculate(Program.definedObjects).Equals(Command.ParseMath("y+1")));   
    }

    [Test]
    public void DefineFunction() {
        Program.START();
        Program.Execute("define(f;x;x^2)");
        Assert.That(Command.ParseMath("f(2)").Calculate(Program.definedObjects).AsValue()==4);
        Assert.That(Command.ParseMath("f").Calculate(Program.definedObjects).Equals(Command.ParseMath("x^2")));
    }


    [Test]
    public void DefineSelfRefVariable() {
        Program.START();
        Program.TryExecute("define(x;x+1)");
        Assert.That(!Program.definedObjects.ContainsKey("x"));
    }

    [Test]
    public void DefineSelfRefFunction() {
        Program.START();
        Program.TryExecute("define(g;g;g+1)");
        Assert.That(!Program.definedObjects.ContainsKey("g"));
        Program.TryExecute("define(g;x;g+1)");
        Assert.That(!Program.definedObjects.ContainsKey("g"));
        Program.TryExecute("define(g;x;g(x)+1)");
        Assert.That(!Program.definedObjects.ContainsKey("g"));
    }
}