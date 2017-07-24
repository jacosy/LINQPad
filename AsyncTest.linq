<Query Kind="Program">
  <Namespace>System.Net</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	"".Dump("ThreadId: " + Thread.CurrentThread.ManagedThreadId);
	Thread.CurrentContext.ContextID.Dump("ContextID");
	
	await Dns.GetHostAddressesAsync("oreilly.com")
		.ContinueWith(IPAddressContinuation);
		
	"".Dump("ThreadId: " + Thread.CurrentThread.ManagedThreadId);
	Thread.CurrentContext.ContextID.Dump("ContextID");
}
// Define other methods and classes here

void IPAddressContinuation(Task<IPAddress[]> taskIPs)
{
	IPAddress[] ips = taskIPs.Result;
	
	foreach (var ip in ips)
	{
		ip.Dump("ThreadId: " + Thread.CurrentThread.ManagedThreadId);
		Thread.CurrentContext.ContextID.Dump("ContextID");
	}
}