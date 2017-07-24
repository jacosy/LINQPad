<Query Kind="Program" />

void Main()
{
	var des = new DelegateAndEventSample { Name = "Origin" };
	// Delegation Sample
	Console.WriteLine("--- Delegation Sample ---");
	DelegateAndEventSample.ToDoSomething delegation = (str) => { Console.WriteLine(str); };
	delegation.Invoke("delegate was invoked from caller");

	Console.WriteLine("");

	// Event Sample
	Console.WriteLine("--- Event Sample ---");
	des.NameChangedHandler += (sender, args) => { Console.WriteLine("First Handler: Caller can design what to do when an event occured but can not invoke it directly."); };
	des.NameChangedHandler += (sender, args) => { Console.WriteLine("Second Handler: The invocation timing of an event is decided by the object contains the event."); };
	des.NameChangedHandler += (sender, args) => { Console.WriteLine($"Third Handler: The new value of the Name of the instance is '{args.CustomObject.ToString()}'"); };
	des.Name = "New Name";
}

// Define other methods and classes here
public class DelegateAndEventSample
{ 
	public delegate void ToDoSomething(string ToDo);
	public event EventHandler<CustomEventArgs> NameChangedHandler;
	
	private string _name;
	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			if (_name != value)
			{
				_name = value;
				if (NameChangedHandler != null)
				{					
					NameChangedHandler(this, new CustomEventArgs { CustomObject = _name });
				}
			}
		}
	}
}

public class CustomEventArgs : EventArgs
{
	public object CustomObject { get; set; }
}