<Query Kind="Program">
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	Console.WriteLine($"{DateTime.Now}	Call DelayTask	ThreadId: {Thread.CurrentThread.ManagedThreadId}");
	CountActiveThread();
	//DelayTask();
	//DelayTask().Wait();
	await DelayTask();
//	DelayTask().ContinueWith(t => { 
//		Console.WriteLine($"DelayTask Status: {t.Status}");
//		Console.WriteLine($"{DateTime.Now}	DelayTask ContinueWith	ThreadId: {Thread.CurrentThread.ManagedThreadId}");
//	});
	//await DelayTask().ContinueWith(t => Console.WriteLine($"DelayTask Status: {t.Status}"));
	CountActiveThread();
	Console.WriteLine($"{DateTime.Now}	Main Finished	ThreadId: {Thread.CurrentThread.ManagedThreadId}");
}

// Define other methods and classes here
private async Task DelayTask()
{
	Console.WriteLine($"{DateTime.Now}	Delay Started	ThreadId: {Thread.CurrentThread.ManagedThreadId}");

	//await Task.Delay(1000);
	Task.Delay(1000).ContinueWith(t => Console.WriteLine($"Task.Delay(1000) is {t.Status}"));
	
	Console.WriteLine($"{DateTime.Now}	Delay Finished	ThreadId: {Thread.CurrentThread.ManagedThreadId}");
	CountActiveThread();
}

private void CountActiveThread()
{
	int count = ((IEnumerable)Process.GetCurrentProcess().Threads)
		.OfType<ProcessThread>()
		.Where(t => t.ThreadState == System.Diagnostics.ThreadState.Running ||
					t.ThreadState == System.Diagnostics.ThreadState.Wait)
		.Count();
	Console.WriteLine($"Current active threads: {count}");
}