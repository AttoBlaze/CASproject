/// <summary>
/// A string tree branch with multiple subbranches/leaves
/// </summary>
public class StringBranch : StringTree {
	public string branchStr;
	public StringTree[] branches;
	public StringBranch(string title, IEnumerable<StringTree> branches, string branchStr = " ^-> ") {
		this.text = title;
		this.branchStr = branchStr;
		this.branches = branches.ToArray();
	}

	public override string Write(int indentation = 0) {
		if(branches.Length==0) return text;
		var newbranch = "\n"+StringOf(' ',indentation)+branchStr;
		return text + newbranch + 
			string.Join(
				newbranch,
				branches.Select(b => b.Write(indentation + branchStr.Length)));
	}
}

/// <summary>
/// A leaf of a string tree; contains no branches.
/// </summary>
public class StringLeaf : StringTree {
	public string title, singleSeperator = ": ", multiSeperator = ":", slabLineJoin = "| ", slabEnd = "╵ ";
	public int slabIndentation = 1;
	public StringLeaf(string text) {
		this.title = "";
		this.text = text;
		this.singleSeperator = "";
		this.multiSeperator = "O";
	}
	public StringLeaf(string title, string text) {
		this.title = title;
		this.text = text;
	}
	
    public override string Write(int indentation = 0) {
		if(text.Contains("\n"))
			return StringTree.MultiLineConvert(title+multiSeperator,text,indentation+slabIndentation,slabLineJoin,slabEnd);
		return title+singleSeperator+text;
	}
}

/// <summary>
/// A tree with string branches. Made to simplify the explain command output, or just trees which need to be written like a tree.
/// </summary>
public class StringTree {
	public string text = "";
	public virtual string Write(int indentation = 0) => StringOf(' ',indentation)+text;

	/// <summary>
	/// Creates a string of the given char with a certain length.
	/// </summary>
	public static string StringOf(char c, int length) {
		char[] chars = new char[length];
		Array.Fill(chars,c);
		return new string(chars);
	}

	/// <summary>
	/// Converts a single text with seperations into a conjoined slab with indentation. <br/>
	/// </summary>
	/// <param name="slabLineJoin">The string used to combine different lines together into a single slab</param>
	/// <param name="slabEnd">The combining string used on the last line of a slab</param>
	public static string MultiLineConvert(string title, string text, int indents, string slabLineJoin ="| ", string slabEnd = "╵ ") {
        string str = "", indent = StringOf(' ',indents);
        string[] lines = text.Split("\n");
        str += title+"\n"+indent+slabLineJoin;
        str += string.Join("\n"+indent+slabLineJoin,lines.SkipLast(1));
        str += "\n"+indent+slabEnd+lines.Last();
        return str;
    }
}