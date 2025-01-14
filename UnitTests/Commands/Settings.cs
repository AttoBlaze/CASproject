namespace CommandTests;

using Application;
using CAS;
using Commands;

public class SettingTests {
    [Test]
    public void SetBoolSetting() {
        Program.START();
        Program.Execute(
            "setSetting(AlwaysWrite;1)",
            "setSetting(AlwaysWrite;true)"
        );
        Program.TryExecute(
            "setSetting(AlwaysWrite;fal)",
            "setSetting(AlwaysWrite;0.3)"
        );
        Assert.That(Program.AlwaysWrite==true);
    }

    [Test]
    public void GetBoolSetting() {
        Program.START();
        Program.AlwaysWrite = true;
        Assert.That(MathObject.Parse("getSetting(AlwaysWrite)").AsValue()==1);
    }
}