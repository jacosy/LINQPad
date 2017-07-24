<Query Kind="Program" />

void Main()
{
	GetMaxReplicate("1234").Dump();
	GetMaxReplicate("1314").Dump();
	GetMaxReplicate("1221").Dump();
	GetMaxReplicate("8366").Dump();
	GetMaxReplicate("2292").Dump();
	GetMaxReplicate("8888").Dump();
}

// Define other methods and classes here
public int GetMaxReplicate(string inputVal)
{
	//	var charAry = inputVal.ToCharArray();
	//	charAry.Dump("inputVal.ToCharArray()");
	//	var charAryGb = charAry.GroupBy(c=>c);
	//	charAryGb.Dump("inputVal.ToCharArray().GroupBy(c=>c)");
	//	var charAryGbMax = charAryGb.Max(group=>group.Count());
	//	charAryGbMax.Dump("inputVal.ToCharArray().GroupBy(c=>c).Max(group=>group.Count())");
	//	return charAryGbMax;

	return inputVal.ToCharArray().GroupBy(c => c).Max(group => group.Count());
}