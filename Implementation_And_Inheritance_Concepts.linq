<Query Kind="Program" />

void Main()
{
	TestChild child = new TestChild();
	child.DoPrint();
	child.DoPrint2();	
	((TestParent)child).DoPrint();
	((TestParent)child).DoPrint2();
	((TestInterface)child).DoPrint();
	((TestInterface)child).DoPrint2();
}

// Define other methods and classes here
interface TestInterface
{ 
	void DoPrint();
	void DoPrint2();
}

public class TestParent : TestInterface
{
	void TestInterface.DoPrint()
	{
		Console.Write(1);
	}

	void TestInterface.DoPrint2()
	{
		Console.Write(3);
	}

	public virtual void DoPrint()
	{
		Console.Write(2);
	}
	public virtual void DoPrint2()
	{
		Console.Write(4);
	}
}

public class TestChild : TestParent
{ 
	public override void DoPrint()
	{
		Console.Write(5);
	}

	public new void DoPrint2()
	{
		Console.Write(6);
	}
}