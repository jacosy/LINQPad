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
	string slnStr = File.ReadAllText("D:\\LINQPad\\FileFolder\\TrafficCopV2API_PCI.sln").Replace(" ", "").Replace(Environment.NewLine, "");

	var reg = new Regex("(?:EndProject)");
//	var reg = new Regex("(?=Project\\([!-\\/:-@\\[-`{-~\\d\\w]*EndProject{1})");
	string[] projList = reg.Split(slnStr);
	projList.Dump();
	projList.Where(p => p.StartsWith("Project") || p.StartsWith("Microsoft Visual Studio Solution File, Format Version"))
		.Select(p =>
		{
			int startIdx = p.IndexOf("=") + 1;
			int endIdx = p.LastIndexOf(",");
			string[] pInfo = p.Substring(startIdx, endIdx - startIdx).Split(',');
			if (pInfo.Length == 2)
			{
				string path = pInfo[1].Replace(@"\", @"\bin\Debug\")
															.Replace("csproj", "dll")
															.Replace("vsproj", "dll");
				//File.Move("D:\\Programs\\"+path, "D:\\Programs\\API_PCI\\DLLs");
				return new
				{
					Name = pInfo[0],
					Path = path
				};
			}
			return null;
		})
		.Where(p => p != null)
		.Dump();
}