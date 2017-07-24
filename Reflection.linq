<Query Kind="Program" />

void Main()
{
	IStep step = StepFactory.CreateStep("ThirdStep");
	Console.Write(step?.PrintSomething());
}

public class StepFactory
{
	public static IStep CreateStep(string stepName)
	{
		// locate the assembly
		var assembly = Assembly.GetExecutingAssembly();
		// get the specific types
        IEnumerable<Type> stepTypes = assembly.GetTypes().Where(t => {
			return (typeof(IStep)).IsAssignableFrom(t) && t.IsClass
				&& t.IsDefined(typeof(StepAttribute));
		}).ToList();
		//activate the type matches to the condition(s)
		Type type = stepTypes.Where(t => t.GetCustomAttribute<StepAttribute>().StepName == stepName).FirstOrDefault();
		return type == null ? null : (Activator.CreateInstance(type) as IStep);
	}
}

// Define other methods and classes here
public class StepAttribute : Attribute
{
	public string StepName { get; set; }
	public StepAttribute(string stepName)
	{
		this.StepName = stepName;
	}
}

public interface IStep
{
	string PrintSomething();
}

[Step("FirstStep")]
public class FirstStep : IStep
{
	public string PrintSomething()
	{
		return "This is FirstStep";
	}
}

[Step("SecondStep")]
public class SecondStep : IStep
{
	public string PrintSomething()
	{
		return "This is SecondStep";
	}
}

[Step("ThirdStep")]
public class ThirdStep : IStep
{
	public string PrintSomething()
	{
		return "This is ThirdStep";
	}
}