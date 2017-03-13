<Query Kind="Program">
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	//var data = await QueryAsync<UserInfo>("ultOC.dbo.uspBPMGetFunDeptManagerByADID", new { AD_ID = "longoriayou" }).ConfigureAwait(false);
	var data = await QueryAsync<object>("declare @test nvarchar(30); select 1;").ConfigureAwait(false);
	data.Dump();
}

// Define other methods and classes here
public async Task<object> QueryAsync<T>(string sqlCmd, object parameters = null, bool isTransaction = false)
{
	object returnData =new Object();
	IDbConnection conn = new SqlConnection("Data Source=192.168.23.53;Initial Catalog=bpmDB;User ID=bpmadmin;Password=2u04y3fu0ck6!@#;MultipleActiveResultSets=True");

	try
	{
		//returnData = await conn.QueryAsync<T>(sqlCmd, param: parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
		returnData = await conn.QueryAsync<T>(sqlCmd, commandType: CommandType.Text).ConfigureAwait(false);
		//returnData = await conn.QueryAsync<T>(sqlCmd, param: parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);			
	}
	catch(Exception ex)
	{
		ex.Dump();	
	}
	finally
	{
		if (!isTransaction)
		{
			if (conn.State == ConnectionState.Open)
			{
				conn.Close();
			}			
		}			
	}
	return returnData;
	
}

public class UserInfo
{
	public string EmplId { get; set; }
	public string Domain { get; set;}
	public string AD_ID { get; set; }
	public string NameDisplay { get; set; }
	public string Email { get; set; }
}