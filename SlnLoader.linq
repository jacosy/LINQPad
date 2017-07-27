<Query Kind="Program" />

void Main()
{
	LoadApisFromPciSln();
	//LoadApisFromNonPciSln();
}

private void LoadApisFromPciSln()
{
	SlnLoader sloader = new SlnLoader(@"C:\TFS Source Code\TrafficCopV2API\TrafficCopV2API_PCI\TrafficCopV2API_PCI.sln");
	sloader.ReadSln()
	//.Where(api => !api.Exist)
	.Dump();
}

private void LoadApisFromNonPciSln()
{
	SlnLoader sloader = new SlnLoader(@"C:\TFS Source Code\TrafficCopV2API\TrafficCopV2API\TrafficCopV2API.sln");
	sloader.ReadSln()
	//.Where(api => !api.Exist)
	.Dump();
}

// Define other methods and classes here
public class SlnLoader
{
	private FileInfo slnFileInfo;
	public string SlnFilePath { get; }

	public SlnLoader(string slnFilePath)
	{
		this.SlnFilePath = slnFilePath;
		this.slnFileInfo = new FileInfo(slnFilePath);
	}

	private string InferDllPath(string projPath)
	{
		StringBuilder sb = new StringBuilder(slnFileInfo.DirectoryName + "\\");
		var projParts = projPath.Split('\\');
		int length = projParts.Length;
		for (int i = 0; i < projParts.Length; i++)
		{
			if (i + 1 == length)
			{
				sb.Append("bin\\Debug\\" + projParts[i].Replace(".csproj", ".dll").Replace(".vbproj", ".dll"));
			}
			else
			{
				sb.Append(projParts[i] + "\\");
			}
		}

		return sb.ToString();		
	}

	private ProjectInfo GetFirstProject(string firstProj)
	{
		int startIndex = firstProj.LastIndexOf("=") + 1;
		int endIndex = firstProj.LastIndexOf(",");
		string[] fpInfo = firstProj.Substring(startIndex, endIndex - startIndex).Split(',');
		string name = fpInfo[0];
		string path = InferDllPath(fpInfo[1]);

		return new ProjectInfo
		{
			Name = fpInfo[0],
			Path = InferDllPath(fpInfo[1]),
			Exist = IsFileExisted(path)
		};
	}

	bool IsFileExisted(string filePath)
	{
		return File.Exists(filePath) ? true : false;
	}

	public IEnumerable<ProjectInfo> ReadSln()
	{
		string slnStr = File.ReadAllText(this.SlnFilePath)
							.Replace(" ", "").Replace("\"", "").Replace(Environment.NewLine, "");

		var reg = new Regex("(?:EndProject)");

		string[] projArr = reg.Split(slnStr);
		string firstProj = projArr.FirstOrDefault();
		//projArr.Dump();
		var projList = projArr.Where(p => p.StartsWith("Project"))
							.Select(p =>
							{
								int startIdx = p.IndexOf("=") + 1;
								int endIdx = p.LastIndexOf(",");
								string[] pInfo = p.Substring(startIdx, endIdx - startIdx).Split(',');
								if (pInfo.Length == 2)
								{
									string name = pInfo[0];
									string path = InferDllPath(pInfo[1]);

									return new ProjectInfo
									{
										Name = name,
										Path = path,
										Exist = IsFileExisted(path)
									};
								}
								return null;
							})
							.Where(p => p != null)
							.ToList();
		//.Dump();
		projList.Add(GetFirstProject(firstProj));
		//projList.Dump();

		return projList;
	}
}

public class ProjectInfo
{
	public string Name { get; set; }
	public string Path { get; set; }
	public bool Exist { get; set; }
}