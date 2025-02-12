namespace CommandTests;

using Application;
using CAS;
using Commands;

public class SettingTests {
    [SetUp] public void start() => Program.START();
	
	[Test]
    public void SetBoolSetting() {
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
        Program.AlwaysWrite = true;
        Assert.That(MathObject.Parse("getSetting(AlwaysWrite)").AsValue()==1);
    }

	[Test]
	public void SetIntSetting() {
		Program.Execute(
			"setSetting(Precision;50)",
			"setSetting(Precision;20*2+10)"
		);
		Program.TryExecute(
			"setSetting(Precision;0.4)",
			"setSetting(Precision;xyz)"
		);
		Assert.That(Program.Precision==50);
	}

	[Test]
	public void GetIntSetting() {
		Program.Precision = 50;
		Assert.That(MathObject.Parse("getSetting(Precision)").AsValue()==50);
	}
}