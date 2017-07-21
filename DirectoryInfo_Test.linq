<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	GetAllApiAssemblies(@"D:\Resources", true);
	
	//var test = new List<string> {
	//	"fvtech.api",
	//	"xml.fvtech",
	//	"TrafficCop.Agent-Global",
	//	"fvtechaa.Api.Xml",
	//	"Api.Xml.fvtech"
	//};
	//ExceptionTest(test);
}

// Define other methods and classes here
private void GetAllApiAssemblies(string folderPath, bool includeSubFolder)
{
	DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
	directoryInfo.EnumerateDirectories(@"ASP.NET", includeSubFolder ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
		.Select(di => di.EnumerateFiles().Select(fi => { Console.WriteLine(fi.Extension); return fi.Extension;}));
		//.Dump();
		//.Where(di => directoryInfo.Name == di.Parent.Name).Select(di => di.Name).Dump();
}

public void ExceptionTest(IEnumerable<string> list)
{
	Regex reg = new Regex("(^fvtech|TrafficCop.Agent-Global|Api.Xml$)", RegexOptions.IgnoreCase);
	list.Where(s=>!reg.IsMatch(s)).Dump();
}