using Commands;
using CAS;
using Application;
using DecimalSharp;
using DecimalSharp.Core;

namespace Development;

public class Tests {
    [Test]
    public void TEST() {
		Program.Execute(
			"define(B;I;(I*8*1.257*10^-6*124)/(5*sqrt(5)*0.15))",
			"define(BR;I;R;B(I)^2*R^2)",
			"define(EM;I;R;U;2*U/(BR(I;R)))"
		);

		string[] data = 
		"5,00	4,64	4,28	3,92	3,55	3,39	3,19 250	274	317	352	392	425	454 0,025	0,03	0,035	0,04	0,045	0,05	0,06"


		.Replace("\t"," ").Split(" ").ToArray();
		var dataAm = data.Length/3;
		var arr = data.Chunk(dataAm).ToArray();
		//Console.WriteLine(string.Join("\n",arr.Select(n => string.Join(" ; ",n))));
		Console.WriteLine("\nB:");
		for(int i=0;i<dataAm;i++) {
			Console.Write((i+1)+": ");
			Program.Execute(
				"B("+arr[0][i]+")"
			);
		}
		Console.WriteLine("\nB^2*r^2:");
		for(int i=0;i<dataAm;i++) {
			Console.Write((i+1)+":");
			Program.Execute(
				"BR("+arr[0][i]+";"+arr[2][i]+")"
			);
		}
		Console.WriteLine("\ne/m:");
		for(int i=0;i<dataAm;i++) {
			Console.Write((i+1)+": ");
			Program.Execute(
				"EM("+arr[0][i]+";"+arr[2][i]+";"+arr[1][i]+")*10^100"
			);
		}
		Assert.Pass();
}
}