<Query Kind="Program" />

void Main()
{
	A obj = new B();
	obj.i = 2;
	B obj1 = new B();
	obj1.j = 10;
	obj.Display();
	obj1.Display();
}

// Define other methods and classes here
public abstract class A
{
	public int i;
	public abstract void Display();
}

public class B : A
{
	public int j;
	public int sum;
	
	public override void Display()
	{
		sum = i + j;
		sum.Dump();
	}
}