<Query Kind="Program">
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\FvTech.Api.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\FvTech.Data.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\FVTech.Imaging.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\FvTech.IO.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\FvTech.Security.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\FvTech.Utility.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\FvTech.Xml.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.Agent-Global.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.Api.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.Api.Xml.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.Batch.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.Configuration.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.Data.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.EOBLockbox-BusinessRuleSetArchiveFilesLocation.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.ErrorHandling.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.Export.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.FDF.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.Form.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.FVF.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.Plugins.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.Remoting.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.Utility.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop.VersionControl.dll</Reference>
  <Reference>C:\Personal Projects\Dlls\TrafficCop.EOBLockbox-BusinessRuleArchiveFiles\bin\Debug\TrafficCop-Global.dll</Reference>
  <Namespace>FvTech.Api</Namespace>
  <Namespace>TrafficCop.Api</Namespace>
  <Namespace>TrafficCop.Plugins</Namespace>
</Query>

void Main()
{
	SlnLoader sloader = new SlnLoader(@"C:\TFS Source Code\TrafficCopV2API\TrafficCopV2API_PCI\TrafficCopV2API_PCI.sln");
	sloader.ReadSln().SelectMany(p =>
	{
		List<ApiClass> apiList = new List<UserQuery.ApiClass>();
		if (p.Exist)
		{
			apiList = LoadFromPath(p.Path);
		}
		return apiList;
	}).Dump();
}

// Define other methods and classes here
private List<ApiClass> GetExportApis(Assembly apiAssembly, string apiNameSpace)
{
	// get the class which impliments BatchEventsBase abstract class and has APIAttribute
	List<ApiClass> apiList = apiAssembly.GetTypes().Where(t =>
	{
		return (typeof(IExport)).IsAssignableFrom(t) && t.IsClass && t.IsDefined(typeof(ExportAttribute));
	}).Select(t =>
	{
		ExportAttribute attr = t.GetCustomAttribute<ExportAttribute>();
		var sType = StepType.None;
		switch ((int)attr.Type)
		{
			case 1:
				sType = StepType.ExportForForm;
				break;
			case 2:
				sType = StepType.ExportForBatch;
				break;
			case 3:
				sType = StepType.ExportForFormAndBatch;
				break;
		}

		return new ApiClass(apiNameSpace)
		{
			ApiType = sType,
			ClassName = t.Name,
			AttrName = attr.Name,
			AttrDescription = attr.Description
		};
	}).ToList();

	return apiList;
}

private List<ApiClass> GetImportApis(Assembly apiAssembly, string apiNameSpace)
{
	// get the class which impliments BatchEventsBase abstract class and has APIAttribute
	List<ApiClass> apiList = apiAssembly.GetTypes().Where(t =>
	{
		return (typeof(IImport)).IsAssignableFrom(t) && t.IsClass && t.IsDefined(typeof(APIAttribute));
	}).Select(t =>
	{
		APIAttribute attr = t.GetCustomAttribute<APIAttribute>();
		return new ApiClass(apiNameSpace)
		{
			ApiType = StepType.Import,
			ClassName = t.Name,
			AttrName = attr.Name,
			AttrDescription = attr.Description
		};
	}).ToList();

	return apiList;
}

private List<ApiClass> GetFormEventApis(Assembly apiAssembly, string apiNameSpace)
{
	// get the class which impliments BatchEventsBase abstract class and has APIAttribute
	List<ApiClass> apiList = apiAssembly.GetTypes().Where(t =>
	{
		return (typeof(IFormEvents)).IsAssignableFrom(t) && t.IsClass && t.IsDefined(typeof(APIAttribute));
	}).Select(t =>
	{
		APIAttribute attr = t.GetCustomAttribute<APIAttribute>();
		return new ApiClass(apiNameSpace)
		{
			ApiType = StepType.FormEvent,
			ClassName = t.Name,
			AttrName = attr.Name,
			AttrDescription = attr.Description
		};
	}).ToList();

	return apiList;
}

private List<ApiClass> GetBatchEventApis(Assembly apiAssembly, string apiNameSpace)
{
	// get the class which impliments BatchEventsBase abstract class and has APIAttribute
	List<ApiClass> apiList = apiAssembly.GetTypes().Where(t =>
	{
		return (typeof(BatchEventsBase)).IsAssignableFrom(t) && t.IsClass && t.IsDefined(typeof(APIAttribute));
	}).Select(t =>
	{
		APIAttribute attr = t.GetCustomAttribute<APIAttribute>();
		return new ApiClass(apiNameSpace)
		{
			ApiType = StepType.BatchEvent,
			ClassName = t.Name,
			AttrName = attr.Name,
			AttrDescription = attr.Description
		};
	}).ToList();

	return apiList;
}

private List<ApiClass> GetDocumentAssemblyApis(Assembly apiAssembly, string apiNameSpace)
{
	// get the class which impliments BatchEventsBase abstract class and has APIAttribute
	List<ApiClass> apiList = apiAssembly.GetTypes().Where(t =>
	{
		return (typeof(IDocumentAssembly)).IsAssignableFrom(t) && t.IsClass && t.IsDefined(typeof(APIAttribute));
	}).Select(t =>
	{
		APIAttribute attr = t.GetCustomAttribute<APIAttribute>();
		return new ApiClass(apiNameSpace)
		{
			ApiType = StepType.DocumentAssembly,
			ClassName = t.Name,
			AttrName = attr.Name,
			AttrDescription = attr.Description
		};
	}).ToList();

	return apiList;
}

private List<ApiClass> GetBusinessRuleApis(Assembly apiAssembly, string apiNameSpace)
{
	var apiList = new List<ApiClass>();

	// get the class which impliments IBusinessRules interface
	IEnumerable<Type> types = apiAssembly.GetTypes()
								.Where(t => (typeof(IBusinessRules)).IsAssignableFrom(t)
										|| (typeof(IBusinessRule)).IsAssignableFrom(t));

	var constFields = new List<FieldInfo>();
	// loop all the types to get const fiels and retrieve their values
	foreach (Type t in types)
	{
		if (t.IsDefined(typeof(APIAttribute)))
		{
			APIAttribute attr = t.GetCustomAttribute<APIAttribute>();
			apiList.Add(new ApiClass(apiNameSpace)
			{
				ApiType = (typeof(IBusinessRules)).IsAssignableFrom(t) ? StepType.BusinessRules : StepType.BusinessRule,
				ClassName = t.Name,
				AttrName = attr.Name,
				AttrDescription = attr.Description
			});
		}
		else
		{
			apiList.Add(new ApiClass(apiNameSpace)
			{
				ApiType = StepType.BusinessRule,
				ClassName = t.Name
			});
		}
	}

	return apiList;
}

public List<ApiClass> LoadFromPath(string filePath)
{
	//var info = new ApiInfo() { ApiDetail = new List<UserQuery.ApiClass>() };
	var apiList = new List<ApiClass>();

	if (!string.IsNullOrEmpty(filePath))
	{
		// load assembly from path
		Assembly apiAssembly = Assembly.LoadFrom(filePath);

		string apiName = apiAssembly.GetName().Name;
		apiList.AddRange(GetBusinessRuleApis(apiAssembly, apiName));
		apiList.AddRange(GetBatchEventApis(apiAssembly, apiName));
		apiList.AddRange(GetFormEventApis(apiAssembly, apiName));
		apiList.AddRange(GetImportApis(apiAssembly, apiName));
		apiList.AddRange(GetExportApis(apiAssembly, apiName));
		apiList.AddRange(GetDocumentAssemblyApis(apiAssembly, apiName));
	}

	return apiList;
}

public class ApiInfo
{
	private string apiName;
	public string ApiName
	{
		get { return apiName; }
		set { apiName = value + ".api"; }
	}
	public List<ApiClass> ApiDetail { get; set; }
}

public class ApiClass
{
	public ApiClass(string apiName)
	{
		this._apiName = apiName;
		this.ApiName = apiName + ".api";
	}
	private string _apiName;
	public readonly string ApiName;
	public string ClassName { get; set; }
	public StepType ApiType { get; set; }
	public string AttrName { get; set; }
	public string ApiDisplayName
	{
		get
		{
			return $"{this._apiName}.{this.AttrName}";
		}
	}
	public string AttrDescription { get; set; }	
}

public enum StepType
{
	None = 0,
	ExportForForm = 1,
	ExportForBatch = 2,
	ExportForFormAndBatch = 3,
	Import,
	BusinessRule,
	BusinessRules,
	BatchEvent,
	FormEvent,
	DocumentAssembly,
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
		//return slnFileInfo.DirectoryName + "\\" + projPath.Replace("\\", "\\bin\\Debug\\").Replace(".csproj", ".dll").Replace(".vbproj", ".dll");
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