namespace Application;
using CAS;

public static partial class Program {
	public static void CreateAllPredefined() {
		Predefine("e",new Constant(Math.E));
        Predefine("pi",new Constant(Math.PI));
        Predefine("log",new FunctionDefinition("log",["base","x"],MathObject.Parse("ln(x)/ln(base)")));
        Predefine("radtodeg",new FunctionDefinition("radtodeg",["radians"],MathObject.Parse("radians*180/pi")));
        Predefine("degtorad",new FunctionDefinition("degtorad",["degrees"],MathObject.Parse("degrees*pi/180")));
	}
}