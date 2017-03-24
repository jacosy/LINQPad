<Query Kind="Program" />

void Main()
{
	var doc = new Dictionary<string, object>
	{
		["@metadata"] = new Dictionary<string, object>
		{
			["@id"] = "users/1"
		}
		["Name"] = "Oren"
	};

	Console.WriteLine(doc["Name"]);
}

// Define other methods and classes here
void Collection_Initializer_Mixed_With_Object_Initializer()
{
	var test = new Dictionary<string, object>
	{
		["Test"] = "Test"
	}
	["Person"] = new { Name = "Longoria", Age = 28 };
	test.Dump();
}