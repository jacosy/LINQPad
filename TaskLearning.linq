<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	Console.WriteLine("Test starts...");
	//await HowManyThreadsUse();
	System.Threading.Thread.CurrentThread.ManagedThreadId.Dump();
	HowManyThreadsUse();
	Console.WriteLine("Test ends (but actually it's not end...)");
}

// Define other methods and classes here
public async Task HowManyThreadsUse()
{
	var t1 = Task.Delay(1000).ContinueWith((task) => Console.WriteLine($"t1 is finished! TaskId: {task.Id}, threadId: {Thread.CurrentThread.ManagedThreadId}"));
	var t2 = Task.Delay(2000).ContinueWith((task) => Console.WriteLine($"t2 is finished! TaskId: {task.Id}, threadId: {Thread.CurrentThread.ManagedThreadId}"));
	var t3 = Task.Delay(3000).ContinueWith((task) => Console.WriteLine($"t3 is finished! TaskId: {task.Id}, threadId: {Thread.CurrentThread.ManagedThreadId}"));
	var t4 = Task.Delay(4000).ContinueWith((task) => Console.WriteLine($"t4 is finished! TaskId: {task.Id}, threadId: {Thread.CurrentThread.ManagedThreadId}"));
	var t5 = Task.Delay(5000).ContinueWith((task) => Console.WriteLine($"t5 is finished! TaskId: {task.Id}, threadId: {Thread.CurrentThread.ManagedThreadId}"));
	var t6 = Task.Delay(6000).ContinueWith((task) => Console.WriteLine($"t6 is finished! TaskId: {task.Id}, threadId: {Thread.CurrentThread.ManagedThreadId}"));
	var t7 = Task.Delay(7000).ContinueWith((task) => Console.WriteLine($"t7 is finished! TaskId: {task.Id}, threadId: {Thread.CurrentThread.ManagedThreadId}"));

	await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7).ContinueWith(task => Console.WriteLine("All Tasks were finished!"));
}