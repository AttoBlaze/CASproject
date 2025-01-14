using Commands;
using CAS;
using Application;

namespace CommandTests;

public class DefineTests{
    [Test]
    public void DefineVariableValue() {
        Program.START();
        Program.Execute("define(x;1)");
        Assert.That(MathObject.Parse("x").Calculate().AsValue()==1);   
    }

    [Test]
    public void DefineVariableExpression() {
        Program.START();
        Program.Execute("define(x;y+1)");
        Assert.That(MathObject.Parse("x").Calculate().Equals(MathObject.Parse("y+1")));   
    }

    [Test]
    public void DefineFunction() {
        Program.START();
        Program.Execute("define(f;y;y^2)");
        Assert.That(MathObject.Parse("f(2)").Calculate().AsValue()==4);
        Assert.That(MathObject.Parse("f").Calculate().Equals(MathObject.Parse("y^2")));
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