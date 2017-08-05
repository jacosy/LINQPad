<Query Kind="Program" />

void Main()
{
	int count = 0, subcount = 0;
	string[] names = { "Tom", "Dick", "Harry", "Mary", "Jay" };
	IEnumerable<string> outerQuery = names
	  .Where(n =>
	  {
	  	count += 1;
		  return n.Length == names.OrderBy(n2 => { subcount+=1; return n2.Length; })
						 .Select(n2 => n2.Length).First();
	  });
	Console.WriteLine($"Before enumerator count: {count}");
	Console.WriteLine($"Before enumerator subcount: {subcount}");
	outerQuery.Dump("Executes query");
	Console.WriteLine($"After enumerator count: {count}");
	Console.WriteLine($"After enumerator subcount: {subcount}");
}

// Define other methods and classes here
