<Query Kind="Program">
  <Connection>
    <ID>0b66375d-15fd-4818-97ba-20875a78edbf</ID>
    <Persist>true</Persist>
    <Server>slc-tsql02</Server>
    <SqlSecurity>true</SqlSecurity>
    <Database>TCV2</Database>
    <UserName>cardiff</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAIiCo71d4r0y05PHSmz5efgAAAAACAAAAAAADZgAAwAAAABAAAADSc1MgJuPwbVzp/OVxzPo3AAAAAASAAACgAAAAEAAAANaCgguYgqyMBD+g62C3jCAIAAAAJR/CsXqVN/MUAAAAjYo/8ZLcKeLwHRxbkDZ8WCjmmyU=</Password>
  </Connection>
</Query>

void Main()
{
	//GetAPIInformation("EOB Patient Pay.wdf");
	//GetAPIInformation("EOB Lockbox Step 1.wdf");
	//GetAPIInformation("EOB Lockbox Step 2.wdf");
	//GetAPIInformation("EOB Lockbox Step 3.wdf");
	GetAPIInformation("EOB Patient Pay PCI.wdf");
	//TestMethod();
	//GetAPIInformationFromFile(@"C:\Users\chuyu\Desktop\New folder\EOB WorkFlow\PCI EOB Patient Pay WorkFlow.xml", "PCI - EOB Patient Pay");
}

// Define other methods and classes here

void TestMethod()
{
	var wdfs = Vw_LatestVersionWorkflowInfo.ToList();
	var wdf = wdfs.FirstOrDefault(w => w.Name == "EOB Patient Pay.wdf");
	var test =
	wdf.XmlContent.Element("Workflow").Elements("Step").Elements("CustomData")?.Elements("DocumentAssembly")?
								.Select(e =>
								{
									return new DocumentAssembly
									{
										Assembly = e.Attribute("Assembly").Value,
										Enabled = e.Attribute("Enabled").Value,
										Exception = e.Attribute("Exception").Value,
										Configuration = string.IsNullOrEmpty(e.Value) ? null : XElement.Parse(e.Value)
									};
								});
	test.Dump();
}

void GetAPIInformation(string wdfName)
{
	var wdfs = Vw_LatestVersionWorkflowInfo.ToList();
	var wdf = wdfs.FirstOrDefault(w => w.Name == wdfName);
	
	if (wdf != null)
	{
		"".Dump(wdfName + " Information");
		"".Dump();
		GetBeforeExportApis(wdf.XmlContent);
		GetAfterExportApis(wdf.XmlContent);
		GetBatchEventApis(wdf.XmlContent);
		GetBatchData(wdf.XmlContent);
		GetMetadata(wdf.XmlContent);
		GetFVFInfo(wdf.XmlContent).Dump("FVF Inforamtion");
		GetWorkFlow(wdf.XmlContent).Dump("WorkFlow Information");
	}
}

void GetAPIInformationFromFile(string wdfPath, string wdfName)
{
	var wdf = XElement.Load(new FileStream(wdfPath, FileMode.Open, FileAccess.Read));
	if (wdf != null)
	{
		"".Dump(wdfName + " Information");
		"".Dump();
		GetBeforeExportApis(wdf);
		GetAfterExportApis(wdf);
		GetBatchEventApis(wdf);
		GetBatchData(wdf);
		GetFVFInfo(wdf).Dump("FVF Inforamtion");
		GetWorkFlow(wdf).Dump("WorkFlow Information");
	}
}

public void GetBeforeExportApis(XElement xelement)
{
	GetExportApis(xelement, "Before");
}

public void GetAfterExportApis(XElement xelement)
{
	GetExportApis(xelement, "After");
}

public void GetExportApis(XElement xelement, string stepType)
{
	xelement.Elements("Exports")
		.Where(e => e.Attribute("Step")?.Value == stepType).Elements("Export")?
		.Select(e =>
		{
			return new Export
			{
				Assembly = e.Attribute("Assembly").Value,
				Enabled = e.Attribute("Enabled").Value,
				Exception = e.Attribute("Exception").Value,
				AllowRepeats = e.Attribute("AllowRepeats").Value,
				UseTrigger = e.Attribute("UseTrigger").Value
			};
		})
		.Dump(stepType + " Export");
}

public void GetBatchEventApis(XElement xelement)
{
	var batchEvents = new BatchEvents();

	batchEvents.Parameters = xelement.Element("BatchEvents").Element("Parameters")?
		.Elements("Parameter")?.Select(e =>
		{
			return new KeyValuePair<string, string>(e.Attribute("Name").Value, e.Value);
		});

	batchEvents.Events = xelement.Element("BatchEvents").Elements("Event")?
		.Select(e =>
		{
			return new BatchEvent
			{
				Assembly = e.Attribute("Assembly").Value,
				Enabled = e.Attribute("Enabled").Value
			};
		});

	batchEvents.Dump("Batch Events");
}

public void GetBatchData(XElement xelement)
{
	xelement.Elements("BatchData").Elements()
		.Where(e => !e.Name.ToString().StartsWith("BatchFld"))
		.Select(e =>
		{
			return new KeyValuePair<XName, string>(e.Name, e.Value);
		})
		.Dump("Custom Fields");
}

public void GetMetadata(XElement xelement)
{
	xelement.Elements("MetadataValues")?.Elements()		
		.Select(e =>
		{
			return new Metadata {
				Name = e.Attribute("Name").Value,
				BatchDataNode = e.Attribute("BatchDataNode").Value,
				DisplayName = e.Attribute("DisplayName").Value
			};
		})
		.Dump("Metadata Mappings");
}

public IEnumerable<DocumentAssembly> GetDocumentAssemblyApis(XElement xelement)
{
	var documentAssemblies = xelement.Elements("DocumentAssembly")?
								.Select(e =>
								{
									return new DocumentAssembly
									{
										Assembly = e.Attribute("Assembly").Value,
										Enabled = e.Attribute("Enabled").Value,
										Exception = e.Attribute("Exception").Value,
										Configuration = string.IsNullOrEmpty(e.Value) ? null : XElement.Parse(e.Value)
									};
								});
	return documentAssemblies;
}

public CustomDataInfo GetCustomData(XElement xelement)
{
	var customData = new CustomDataInfo { Settings = new Dictionary<string, string>() };
	foreach (var attr in xelement.Attributes())
	{
		customData.Settings.Add(attr.Name.LocalName, attr.Value);
	}

	if (xelement.Elements("BusinessRules").Count() > 0)
	{
		customData.StepTasks = GetBusinessRules(xelement);
	}
	if (xelement.Elements("DocumentAssembly").Count() > 0)
	{
		customData.StepTasks = GetDocumentAssemblyApis(xelement);
	}
	if (xelement.Elements("PreprocessingList").Count() > 0)
	{
		customData.StepTasks = GetPreprocessings(xelement);
	}

	return customData;
}

public IEnumerable<BusinessRule> GetBusinessRules(XElement xelement)
{
	var businessRules = xelement.Elements("BusinessRules")?
							.Select(e =>
							{
								return new BusinessRule
								{
									Rule = e.Attribute("Rule").Value,
									Enabled = e.Attribute("Enabled").Value,
									Exception = e.Attribute("Exception").Value,
									Configuration = string.IsNullOrEmpty(e.Value) ? null : XElement.Parse(e.Value)
								};
							});
	return businessRules;
}

public IEnumerable<Preprocessing> GetPreprocessings(XElement xelement)
{
	var preProcessings =
		xelement.Elements("PreprocessingList")?.Elements("Process")
			.Select(e =>
			{
				return new Preprocessing
				{
					Process = e.Attribute("Name").Value,
					Crop = e.Attribute("Crop")?.Value,
					Method = e.Attribute("Method")?.Value,
					XResize = e.Attribute("XResize")?.Value,
					YResize = e.Attribute("YResize")?.Value
				};
			});
	return preProcessings;
}

public IEnumerable<StepInfo> GetWorkFlow(XElement xelement)
{
	var steps = xelement.Element("Workflow").Elements("Step")?
						.Select(e =>
						{
							var step = new StepInfo
							{
								Index = e.Attribute("Index").Value,
								TASK = e.Attribute("TASK").Value,
								TIMEOUT = e.Attribute("TIMEOUT").Value,
								StopPoint = e.Attribute("StopPoint").Value,
								Distributed = e.Attribute("Distributed").Value,
								CacheSize = e.Attribute("CacheSize").Value,
								AllowSkip = e.Attribute("AllowSkip").Value,
								Description = e.Attribute("Description").Value
							};

							if (e.HasElements)
							{
								step.CustomData = GetCustomData(e.Element("CustomData"));
							}

							return step;
						});
	return steps;
}

public FVFList GetFVFInfo(XElement xelement)
{
	var fvfList = new FVFList();
	
	fvfList.FVFs = xelement.Element("FVFList")?.Elements("FVF")?
							.Select(e => e.Attribute("Name").Value);
	fvfList.DefaultFVF = xelement.Element("DefaultFVF")?.Value;
	
	return fvfList;
}

public class StepInfo
{
	public string Index { get; set; }
	public string TASK { get; set; }
	public string TIMEOUT { get; set; }
	public string StopPoint { get; set; }
	public string Distributed { get; set; }
	public string CacheSize { get; set; }
	public string AllowSkip { get; set; }
	public string Description { get; set; }
	public CustomDataInfo CustomData { get; set; }
}

public class CustomDataInfo
{
	public Dictionary<string, string> Settings { get; set; }
	public IEnumerable<IStep> StepTasks { get; set; }
	public string TaskSettings
	{
		get
		{
			StringBuilder returnStr = new StringBuilder();
			foreach (var element in this.Settings)
			{
				returnStr.AppendLine(string.Format("{0} = {1}", element.Key, element.Value));
			}
			return returnStr.ToString();
		}
	}

	object ToDump() => new { TaskSettings, StepTasks };
}

public interface IStep
{
	XElement Configuration { get; set; }
}

public class Export : IStep
{
	public string Assembly { get; set; }
	public string Enabled { get; set; }
	public string Exception { get; set; }
	public string AllowRepeats { get; set; }
	public string UseTrigger { get; set; }
	public XElement Configuration { get; set; }
	
	object ToDump() => new { Assembly, Enabled, Exception, AllowRepeats, UseTrigger };
}

public class BusinessRule : IStep
{
	public string Rule { get; set; }
	public string Enabled { get; set; }
	public string Exception { get; set; }
	public XElement Configuration { get; set; }
	
	object ToDump() => new { Rule, Enabled, Exception };
}

public class DocumentAssembly : IStep
{
	public string Assembly { get; set; }
	public string Enabled { get; set; }
	public string Exception { get; set; }
	public XElement Configuration { get; set; }
	
	object ToDump() => new { Assembly, Enabled, Exception };
}

public class Preprocessing : IStep
{
	public string Process { get; set; }
	public string Crop { get; set; }
	public string Method { get; set; }
	public string XResize { get; set; }
	public string YResize { get; set; }
	public XElement Configuration { get; set; }
	
	object ToDump() => new { Process, Crop, Method, XResize, YResize };
}

public class BatchEvent
{
	public string Assembly { get; set; }
	public string Enabled { get; set; }
}

public class BatchEvents
{
	public IEnumerable<KeyValuePair<string, string>> Parameters { get; set; }
	public IEnumerable<BatchEvent> Events { get; set; }
}

public class FVFList
{
	public string DefaultFVF { get; set; }
	public IEnumerable<string> FVFs {get; set;}
}

public class Metadata
{
	public string Name { get; set; }
	public string BatchDataNode { get; set; }
	public string DisplayName { get; set; }
}