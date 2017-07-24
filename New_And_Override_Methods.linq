<Query Kind="Program" />

void Main()
{
	BaseClass bc = new BaseClass();
	DerivedClass dc = new DerivedClass();
	BaseClass bcdc = new DerivedClass();

	Console.WriteLine("-- bc --");
	bc.Method1();  
	bc.Method2();
	Console.WriteLine("-- dc --");
	dc.Method1();
	dc.Method2();
	Console.WriteLine("-- bcdc --");
	bcdc.Method1();
	bcdc.Method2();
}

public class BaseClass
{
	public virtual void Method1()
	{
		Console.WriteLine("Base - Method1");
	}

	public void Method2()
	{
		Console.WriteLine("Base - Method2");
	}
}

public class DerivedClass : BaseClass
{
	public override void Method1()
	{
		Console.WriteLine("Derived - Method1");
	}

	public new void Method2()
    {  
        Console.WriteLine("Derived - Method2");
	}
}