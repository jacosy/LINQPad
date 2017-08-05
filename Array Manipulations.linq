<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	int[] squares = { 1, 4, 9, 16 };
	//squares.Dump("squares: before copy is modified.");

	int[] copy = new int[4];
	squares.CopyTo(copy, 0);
	copy[2] = 0;
	squares.Dump("squares: after copy is modified.");
	copy.Dump("copy dump");
	var comparer = new NumberComparer();
	Array.Sort(copy, comparer);
	Array.IndexOf(copy, 16).Dump();
	//copy.Dump("the final dump");
}

// Define other methods and classes here
public class NumberComparer : Comparer<int>
{ 
	public override int Compare(int x, int y)
	{
		return x.CompareTo(y);
	}
}