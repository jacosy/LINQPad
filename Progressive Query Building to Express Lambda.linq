<Query Kind="Program" />

void Main()
{
	IEnumerable<int> 
  source = new int[] { 5, 12, 3 },
  filtered = source.Where(n => n < 10),
  sorted = filtered.OrderBy(n => n),
  query = sorted.Select(n => n * 10);
  
  query.Dump();
}

// Define other methods and classes here
