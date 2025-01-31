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

public class StringLeaf : StringTree {
	public string title, singleSeperator = ": ", multiSeperator = ":";
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
			return StringTree.MultiLineConvert(title+multiSeperator,text,indentation+1);
		return title+singleSeperator+text;
	}
}

/// <summary>
/// A tree with string branches. Made to simplify the explain command output.
/// </summary>
public class StringTree {
	public string text = "";
	public virtual string Write(int indentation = 0) => StringOf(' ',indentation)+text;

	public static string StringOf(char c, int length) {
		char[] chars = new char[length];
		Array.Fill(chars,c);
		return new string(chars);
	}

	public static string MultiLineConvert(string title, string text, int indents) {
        string str = "", indent = StringOf(' ',indents);
        string[] lines = text.Split("\n");
        str += title+"\n"+indent+"| ";
        str += string.Join("\n"+indent+"| ",lines.SkipLast(1));
        str += "\n"+indent+"╵ "+lines.Last();
        return str;
    }
}