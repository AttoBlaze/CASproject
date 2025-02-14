namespace Application;
using CAS;
using Commands;

public static partial class Program {
	public static void CreateAllPredefined() {
		Predefine("e",Calculator.e);
        Predefine("pi",Calculator.pi);
        Predefine("log",new FunctionDefinition("log",["base","x"],MathObject.Parse("ln(x)/ln(base)")));
        Predefine("radtodeg",new FunctionDefinition("radtodeg",["radians"],MathObject.Parse("radians*180/pi")));
        Predefine("degtorad",new FunctionDefinition("degtorad",["degrees"],MathObject.Parse("degrees*pi/180")));
        Predefine("sqrt",new FunctionDefinition("sqrt",["x"],new Power((Variable)"x",(Constant)0.5)));
	    Predefine("cbrt",new FunctionDefinition("cbrt",["x"],new Power((Variable)"x",new Divide((Constant)1,(Constant)3))));
    }
}