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
	

	/// <inheritdoc cref="WriteStylized(object,bool,bool)"></inheritdoc>
	public void Write(object obj) => WriteStylized(obj,false);
	/// <inheritdoc cref="WriteStylized(object,bool,bool)"></inheritdoc>
	public void WriteLine(object obj) => WriteStylized(obj,true);
	/// <summary>
	/// Writes in the console with this styling. The console style is reverted to normal after use. 
	/// </summary>
	public void WriteStylized(object obj, bool newLine = false, bool resetStyling = true) {
		this.apply();
		if(newLine) Console.WriteLine(obj);
		else		Console.Write(obj);
		if(resetStyling) Current.apply();
	}

	/// <summary>
	/// Applies this styling to the console.
	/// </summary>
	public void Apply() {
		apply();
		Current = this;
	}

	/// <summary>
	/// Applies this styling to the console without changing the current styling
	/// </summary>
	private void apply() {
		//reset styling
		ResetConsoleStyle();

		//text
		if(textColor is Color text) //dont change if null
			Console.Write("\x1b[38;2;"+text.R+";"+text.G+";"+text.B+"m");						
		
		//background
		if(backgroundColor is Color background) //dont change if null	
			Console.Write("\x1b[48;2;"+background.R+";"+background.G+";"+background.B+"m");		
		
		//font stylings
		foreach(var fontStyling in fontStylings)
			Console.Write("\x1b["+(int)fontStyling+"m");												
	}

	/// <summary>
	/// Resets the styling of the console to its default.
	/// </summary>
	public static void ResetConsoleStyle() => Console.Write("\x1b[0m");
	
	public static implicit operator ConsoleStyling(Color col) => new(col);
	public static implicit operator ConsoleStyling(ConsoleFontStyling col) => new(col);
}