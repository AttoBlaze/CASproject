using Commands;
using CAS;
using Application;
using DecimalSharp;
using DecimalSharp.Core;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
        Program.START();
        Program.Execute(
			"ln(2)",
            "setSetting(UseDouble;false)",
			"ln(2)",
            "setSetting(Precision;75)",
			"ln(2)"
        );
		
		Constant? temp = null,temp1 = null; 
		Program.Log(GetTime.Time(()=> {
			temp = CASMath.Log(CASMath.Divide(1,3,true),true);
		}));
		Program.Log(GetTime.Time(()=> {
			temp1 = CASMath.Log(CASMath.Divide(1,3,false),false);
		}));
		Program.Log(temp?.AsString()+"\n"+temp1?.AsString());

		Assert.Pass();
    }
}