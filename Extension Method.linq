<Query Kind="Program" />

void Main()
{
	string test = "test";
	string doubleStr = "59.884";
	// ToDouble Extension Method (without parameter)
	test.ToDouble().Dump("The result of string value executed ToDouble method:");
	(-0.001 > test.ToDouble()).Dump("The result that the return value of string execute ToDouble method compares to negative double value:");
	doubleStr.ToDouble().Dump("The result that the string value which can be converted to double execute ToDouble method:");
	
	// IsTypeEqual Extension Method (with parameter)
	test.IsTypeEqual(typeof(double)).Dump("string is equal to double?");
	doubleStr.ToDouble().IsTypeEqual(typeof(double)).Dump("double is equal to double?");
}

// Define other methods and classes here
public static class StringExtensions
{
	public static double ToDouble(this string data)
	{
		double result;
		return double.TryParse(data, out result) ? result : double.NaN;
	}
	
	public static bool IsTypeEqual(this object obj, Type type)
	{
		return obj.GetType() == type;
	}
}