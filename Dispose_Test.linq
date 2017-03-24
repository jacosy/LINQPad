<Query Kind="Program" />

void Main()
{
	var disposeTest = new ClassForDisposeTest();
	Console.WriteLine("This line is just for checking the Reader property of disposeTest object");
	disposeTest.Dispose();
}

// Define other methods and classes here
public class ClassForDisposeTest : IDisposable
{
	public StreamReader Reader { get; set; }
	public ClassForDisposeTest()
	{
		Reader = new StreamReader(@"D:\eula.1028.txt");
	}
	public void Dispose()
	{
		Reader.Dispose();
	}
}