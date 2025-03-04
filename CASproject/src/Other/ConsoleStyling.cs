using System.Drawing;

/// <summary>
/// A styling of the console font. <br/>
/// Utilizes ANSI codes.
/// </summary>
public enum ConsoleFontStyling {
	Plain = 10,
	Italic = 3,
	Bold = 1,
	Faint = 2,
	Underlined = 4,
	Strikethrough = 9,
	Overlined = 55
}

/// <summary>
/// Represents a styling of the console. Used streamline the process of writing in the console with different fonts and colors. <br/>
/// Utilizes ANSI codes.
/// </summary>
public class ConsoleStyling {
	/// <summary>
	/// The default styling of the console.
	/// </summary>
	public static readonly ConsoleStyling Plain  = new();
	/// <summary>
	/// The current styling of the console.
	/// </summary>
	public static ConsoleStyling Current {get; private set;} = Plain;
	
	
	public Color? textColor, backgroundColor;
	public HashSet<ConsoleFontStyling> fontStylings = [];
	
	
	public ConsoleStyling(params ConsoleFontStyling[] fontStylings) : this(null,null,fontStylings) {}
	public ConsoleStyling(Color textColor, params ConsoleFontStyling[] fontStylings) : this(textColor,null,fontStylings) {}
	public ConsoleStyling(Color? textColor = null, Color? backgroundColor = null, params ConsoleFontStyling[] fontStylings) {
		this.textColor = textColor;
		this.backgroundColor = backgroundColor;
		this.fontStylings = fontStylings.ToHashSet();
	}
	

	/// <inheritdoc cref="WriteStylized(object,bool,bool)"/>
	public void Write(object obj) => WriteStylized(obj,false);
	
	/// <inheritdoc cref="WriteStylized(object,bool,bool)"/>
	public void WriteLine(object obj) => WriteStylized(obj,true);
	
	/// <summary>
	/// Writes in the console with this styling. The console style is reverted to normal after use. 
	/// </summary>
	public void WriteStylized(object obj, bool newLine = false, bool resetStyling = true) {
		string msg = Stylize(obj,resetStyling);

		//write stylized message
		if(newLine) Console.WriteLine(msg);
		else Console.Write(msg);
	}

	/// <summary>
	/// Gets a string which is written with this styling. The console style is reverted back to the "ConsoleStyling.Current" styling after this string unless specified.
	/// </summary>
	public string Stylize(object obj, bool resetStyling = true) {
		//create stylized message
		string msg = GetStyling() + obj;

		//reset styling after message if specified
		if(resetStyling) msg += Current.GetStyling();
		return msg;
	}

	/// <summary>
	/// Applies this styling to the console.
	/// </summary>
	public void Apply() {
		Console.Write(GetStyling());
		Current = this;
	}

	/// <summary>
	/// Gets the string which, when written in the console, applies this styling to the console.
	/// </summary>
	public string GetStyling() {
		//reset styling
		string styling = GetStylingReset();

		//text color
		if(textColor is Color text) //dont change if null
			styling += "\x1b[38;2;"+text.R+";"+text.G+";"+text.B+"m";						
		
		//background color
		if(backgroundColor is Color background) //dont change if null	
			styling += "\x1b[48;2;"+background.R+";"+background.G+";"+background.B+"m";		
		
		//font stylings
		foreach(var fontStyling in fontStylings)
			styling += "\x1b["+(int)fontStyling+"m";

		return styling;												
	}

	/// <summary>
	/// Resets the styling of the console to its default.
	/// </summary>
	public static void ResetConsoleStyling() => Plain.Apply();
	
	/// <summary>
	/// Gets the string which, when written in the console, resets the styling.
	/// </summary>
	public static string GetStylingReset() => "\x1b[0m";
	
	//implicit casts
	public static implicit operator ConsoleStyling(Color textColor) => new(textColor);
	public static implicit operator ConsoleStyling(ConsoleFontStyling fontStyle) => new(fontStyle);
}

public static class ConsoleStylingExtensions {
	/// <inheritdoc cref="ConsoleStyling.Stylize(object, bool)"/>
	public static string StylizeInConsole(this string obj, ConsoleStyling style, bool resetStyling = true) =>
		style.Stylize(obj,resetStyling);
}