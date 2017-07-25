<Query Kind="Program" />

void Main()
{
	XDocument xmlDoc = XDocument.Load(@"D:\PDF\Google 雲端硬碟\CodeSnippet\LINQPad\FileFolder\workflow.xml");
	GetBeforeExportApis(xmlDoc, "TrafficCop.EobLockbox-BatchStatsBeforeExport.api");
	GetAfterExportApis(xmlDoc, "TrafficCop.EobLockbox-BatchStatsExport.api");
	GetBatchEventApis(xmlDoc, "TrafficCop.EOBLockbox-BatchEventsClaimsClipStitch.api");
	GetDocumentAssemblyApis(xmlDoc, "TrafficCop.Plugins-DocumentAssemblyPage.api");
}

// Define other methods and classes here
public void GetBeforeExportApis(XDocument xmlDoc, string apiName)
{
	GetExportApis(xmlDoc, "Before", apiName);
}

public void GetAfterExportApis(XDocument xmlDoc, string apiName)
{
	GetExportApis(xmlDoc, "After", apiName);
}

public void GetExportApis(XDocument xmlDoc, string stepType, string apiName)
{
	int index = 0;
	xmlDoc.Element("Job").Elements("Exports")
		.Where(e => e.Attribute("Step")?.Value == stepType).Elements("Export")?
		.Where(e =>
		{
			bool result = false;
			index += 1;
			if (e.Attribute("Assembly")?.Value == apiName)
			{
				e.Add(new XAttribute("ExecutionOrder", index));
				result = true;
			}			
			return result;
		})
		.Select(e =>
		{
			return e.Attributes().Select(attr => new KeyValuePair<string, string>(attr.Name.ToString(), attr.Value));
		})
		.Dump();
}

public void GetBatchEventApis(XDocument xmlDoc, string apiName)
{
	int index = 0;
	xmlDoc.Element("Job").Element("BatchEvents").Elements("Event")?
		.Where(e => {
			bool result = false;
			index += 1;
			if (e.Attribute("Assembly")?.Value == apiName)
			{
				e.Add(new XAttribute("ExecutionOrder", index));
				result = true;
			}
			return result;
		})
		.Select(e => {
			return e.Attributes().Select(attr => new KeyValuePair<string, string>(attr.Name.ToString(), attr.Value));
		})
		.Dump();
}

public void GetDocumentAssemblyApis(XDocument xmlDoc, string apiName)
{
	int index = 0;
	xmlDoc.Element("Job").Element("Workflow").Elements("Step")?
		.Where(e=>e.Attribute("TASK")?.Value == "DocumentAssembly").Elements("CustomData")?.Elements("DocumentAssembly")?
		.Where(e =>
		{
			bool result = false;
			index += 1;
			if (e.Attribute("Assembly")?.Value == apiName)
			{
				e.Add(new XAttribute("ExecutionOrder", index));
				result = true;
			}
			return result;
		})
		.Select(e =>
		{
			return e.Attributes().Select(attr => new KeyValuePair<string, string>(attr.Name.ToString(), attr.Value));
		})
		.Dump();
}

// Define other methods and classes here
public class ApiInfo
{
	private string apiName;
	public string ApiName
	{
		get { return apiName; }
		set { apiName = value + ".api"; }
	}
}

public class WorkFlow
{
	public string WorkFlowName { get; set; }
	public List<string> BeforeExports { get; set; }
	public List<string> AfterExports { get; set; }
	public List<string> BatchEvents { get; set; }
	public List<string> DocumentAssembly { get; set; }
}