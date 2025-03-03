using Application;

internal class MAIN {
    static void Main() {
        //start program
		Program.START();

		//style console for inputs
		new ConsoleStyling(System.Drawing.Color.WhiteSmoke).Apply(); 

        //input reader
        while(true) {
            string? input = Console.ReadLine();
            if (input==null) continue;
            Program.TryExecute(input);
        }       
    }
}

public static class Extensions {
    public static bool IsNumericType(this object o) {   
        switch (Type.GetTypeCode(o.GetType())) {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
            return true;
            default:
            return false;
        }
    }
}