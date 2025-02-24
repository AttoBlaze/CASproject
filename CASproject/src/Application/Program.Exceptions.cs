namespace Application;

public class InputCountException : Exception {
	public InputCountException(string detectedFor, int expected, int actual) : 
		base("Improper input count for "+detectedFor+"! (Input count must be "+expected+", actual is "+actual+")") {}

	public InputCountException(string detectedFor, string expected, int actual) : 
		base("Improper input count for "+detectedFor+"! (Input count must be "+expected+", actual is "+actual+")") {}

	public InputCountException(string message) : base(message) {}
}