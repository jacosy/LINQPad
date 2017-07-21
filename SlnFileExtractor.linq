<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	SlnReader();
	//RegexTest();
	//StringSplitMethod();
}

// Define other methods and classes here
void StringSplitMethod()
{	
	var test = "TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\\TrafficCop.EOBLockbox-BusinessRuleSetArchiveFilesLocation.csproj";
	test.Split('\\').Dump();
}

void RegexTest()
{
	var testStr = @"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""TrafficCop.Plugins-Common"", ""TrafficCop.Plugins-Common\\TrafficCop.Plugins-Common.csproj"", ""{5A9A88D9-B762-49C6-8140-35EDFC8E6580}EndProject".Replace(" ", "").Replace("\"", "");	
	var reg = new Regex("(?:^Project|EndProject$)");
	
	string projStr = reg.Replace(testStr, "").Replace("\"", "");
	
	int startIdx = projStr.IndexOf("=") + 1;
	int endIdx = projStr.LastIndexOf(",");
	
	projStr.Substring(startIdx, endIdx - startIdx).Dump();
	
	reg.Split(testStr).Dump();
}

void SlnReader()
{
	string slnStr = File.ReadAllText("D:\\Programs\\TrafficCopV2API_PCI.sln").Replace(" ", "").Replace("\"", "");
	
	var reg = new Regex("(?:EndProject)");
	
	string[] projList = reg.Split(slnStr);
	projList.Where(p => p.StartsWith(Environment.NewLine + "Project"))
		.Select(p =>
		{
			int startIdx = p.IndexOf("=") + 1;
			int endIdx = p.LastIndexOf(",");
			string[] pInfo = p.Substring(startIdx, endIdx - startIdx).Split(',');
			if (pInfo.Length == 2)
			{
				return new { Name = pInfo[0], Path = pInfo[1] };
			}
			return null;
		})
		.Where(p=>p != null)
		.Dump();

//	projList.Dump();	
//	projList.Select(p => {
//		p.Split(',')
//	});
//	int startIdx = projStr.IndexOf("=") + 1;
//	int endIdx = projStr.LastIndexOf(",");
//
//	projStr.Substring(startIdx, endIdx - startIdx).Dump();
//
//	reg.Split(slnStr).Dump();
}