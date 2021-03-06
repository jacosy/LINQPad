<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Security.Principal</Namespace>
</Query>

void Main()
{
	Console.WriteLine("------ Test Start ------");
	Console.WriteLine(GetThreadInformation());
	//Task.Run(()=> Console.WriteLine("task was done!"));
	var logger = new SimpleLog(WindowsIdentity.GetCurrent().Name.Split('\\')[1]);	
	logger.LogAsync();
	Console.WriteLine("------ LogAsync Starts ------\n");
	Console.WriteLine(GetThreadInformation());
	Console.WriteLine("------ Test End ------");
}

public static string GetThreadInformation()
{
	StringBuilder sb = new StringBuilder();
	sb.AppendLine($"Current Thread ID: {Thread.CurrentThread.ManagedThreadId}");
	sb.AppendLine($"Current Thread Name: {Thread.CurrentThread.Name}");
	sb.AppendLine($"Current Thread IsBackground: {Thread.CurrentThread.IsBackground}");
	sb.AppendLine($"Current Thread IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");
	sb.AppendLine($"CurrentContext ID: {Thread.CurrentContext.ContextID}");
	//sb.AppendLine($"Current Thread ExecutionContext: {Thread.CurrentThread.ExecutionContext}");
	return sb.ToString();
}

// Define other methods and classes here
public class SimpleLog
{
	private static readonly string _filePath = @"D:\Temp\LogTest\";
	public string LogUser { get; set;}
	private string _fileName;
	private string _fullFileName;
		
	static SimpleLog()
	{
		
	}
	
	public SimpleLog(string logUser)
	{
		LogUser = logUser;
		this._fileName = $"{DateTime.Today.ToString("yyyyMMdd")}_{this.LogUser}.txt";
		this._fullFileName = Path.Combine(_filePath, this._fileName);
	}

	public void WriteToFile(string logInfo)
	{
		var fi = new FileInfo(this._fullFileName);
        if (!fi.Exists)
		{			
			using (StreamWriter sw = fi.CreateText())
			{
				sw.WriteLine(logInfo);
			}
		}
		else
		{
			using (var fs = new FileStream(this._fullFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 4096, FileOptions.Asynchronous))
			{
				byte[] encodedText = Encoding.UTF8.GetBytes(logInfo);
				fs.Write(encodedText, 0, encodedText.Length);
			}
		}
	}

	public void LogAsync()
	{
		Task.Delay(2000).ContinueWith(t =>
		{
			var logInfo = new StringBuilder();
			logInfo.AppendLine("Log Action: Log");
			logInfo.AppendLine($"LogUser: {this.LogUser}");
			logInfo.AppendLine($"LogTime: {DateTime.Now}");
			logInfo.AppendLine(GetThreadInformation());
			logInfo.AppendLine();						
			WriteToFile(logInfo.ToString());			
			Console.WriteLine("Log Task was done!");
		}).ConfigureAwait(false);
	}
}