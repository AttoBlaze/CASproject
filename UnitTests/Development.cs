using Commands;
using CAS;
using Application;
using DecimalSharp;
using DecimalSharp.Core;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
		Constant? temp = null,temp1 = null; 
		var calc = new CASMath();
		Console.WriteLine(CASMath.Calculator.add(1,1));

		Program.Log(GetTime.Time(()=> {
			temp = CASMath.Log(CASMath.Divide(1,3));
		}));
		Program.Log(GetTime.Time(()=> {
			temp1 = CASMath.Log(CASMath.Divide(1,3));
		}));
		Program.Log(temp?.AsString()+"\n"+temp1?.AsString());
        
		Program.START();
        Program.Execute(
			"setSetting(ArbitraryPrecision;false)",
			"ln(2)",
            "setSetting(ArbitraryPrecision;true)",
			"ln(2)",
            "setSetting(Precision;75)",
			"ln(2)"
        );
		

		Assert.Pass();
    }
}