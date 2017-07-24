<Query Kind="Program">
  <Connection>
    <ID>f2f6e2b4-0f40-48df-b2c7-82584a77c393</ID>
    <Server>(LocalDb)\MSSQLLocalDB</Server>
    <AttachFile>true</AttachFile>
    <AttachFileName>&lt;ApplicationData&gt;\LINQPad\Nutshell.mdf</AttachFileName>
    <Persist>true</Persist>
  </Connection>
  <Namespace>System.Data.Linq</Namespace>
  <Namespace>System.Data.Linq.Mapping</Namespace>
</Query>

void Main()
{
	RetrieveCustomerByCriterias();
}

// Define other methods and classes here
public void RetrieveCustomerByCriterias()
{
	DataContext dataContext = new DataContext(@"Server=(localDb)\MSSQLLocalDB;Database=C:\Users\longoria\AppData\Roaming\LINQPad\Nutshell.mdf;Trusted_Connection=True;");
	Table<Customer> customers = dataContext.GetTable<Customer>();

	IQueryable<string> query = from c in customers
							   where c.Name.Contains("a")
							   orderby c.Name.Length
							   select c.Name.ToUpper();

	foreach (string name in query)
	{
		Console.WriteLine(name);
	}
}

[Table]
public class Customer
{
	[Column(IsPrimaryKey = true)] public int ID;
	[Column] public string Name;
}