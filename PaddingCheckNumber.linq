<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	DataTable dt = MockObjects.GetData();
	var br = new BusinessRuleEOBLockboxPostQCPaddingCheckNumber("33099,10,^0|LBW3CO,8,$z|27154,4,|27154,4,^|27154,4,  |test|test,");
	br.PaddingCheckNumber(dt);
	dt.Dump();
}

public class MockObjects
{
	public static DataTable GetData()
	{
		DataTable dt = new DataTable();
		dt.Columns.Add("PayerID");
		dt.Columns.Add("CheckNumber");

		dt.Rows.Add("33099", "896D111111");
		dt.Rows.Add("33099", "222222896D");
		dt.Rows.Add("33099", "896D333333896D");
		dt.Rows.Add("33099", "444444");
		dt.Rows.Add("33099", "2T5559999");
		dt.Rows.Add("27154", "AB6666");
		dt.Rows.Add("27154", "S");
		dt.Rows.Add("27154", "st");
		dt.Rows.Add("LBW3CO", "234");
		dt.Rows.Add("LBW3CO", "7777CCC");
		dt.Rows.Add("LBW3CO", "88888XXX");
		dt.Rows.Add("LBW3CO", "99");
		dt.Rows.Add("LBW3CO", "999");
		dt.Rows.Add("test", "t e s t");
		return dt;
	}
}

public enum PadDirection
{
	PadLeft,
	PadRight,
	None
}

public class PaddingRuleModel
{
	public string PayID { get; set; }
	public int RequireLength { get; set; }
	public PadDirection PadPattern { get; set; }
	public char Character { get; set; }
}

// Define other methods and classes here
public class BusinessRuleEOBLockboxPostQCPaddingCheckNumber
{
	public List<PaddingRuleModel> RuleList { get; private set; }

	public BusinessRuleEOBLockboxPostQCPaddingCheckNumber(string ruleString)
	{
		SetRuleList(ruleString);
	}

	private void SetRuleList(string ruleString)
	{
		RuleList = new List<PaddingRuleModel>();
		string[] rules = ruleString.Split('|');
		foreach (string ruleStr in rules)
		{
			string[] ruleArr = ruleStr.Split(',');
			int totalLength;
			char[] paddingRule;
			if (ruleArr.Length == 3 && int.TryParse(ruleArr[1], out totalLength) && ruleArr[2].Trim().Length == 2)
			{
				paddingRule = ruleArr[2].ToCharArray();
				RuleList.Add(new PaddingRuleModel
				{
					PayID = ruleArr[0],
					RequireLength = totalLength,
					PadPattern = paddingRule[0] == '^' ? PadDirection.PadLeft
						: paddingRule[0] == '$' ? PadDirection.PadRight : PadDirection.None,
					Character = Convert.ToChar(paddingRule[1])
				});
			}
		}
	}

	private IPaddingCharacterBehavior GetPaddingCharacterBehavior(PaddingRuleModel rule)
	{
		IPaddingCharacterBehavior paddingBehavior = null;
		if (rule.PadPattern == PadDirection.PadLeft)
		{
			paddingBehavior = new PaddingLeftCharacterBehavior(rule.RequireLength, rule.Character);
		}
		else if (rule.PadPattern == PadDirection.PadRight)
		{
			paddingBehavior = new PaddingRightCharacterBehavior(rule.RequireLength, rule.Character);
		}
		return paddingBehavior;
	}

	public void PaddingCheckNumberByPayerId(DataTable dt, PaddingRuleModel rule)
	{
		DataRow[] drs = dt.Select(string.Format("PayerID = '{0}'", rule.PayID));
		IPaddingCharacterBehavior padBehavior = GetPaddingCharacterBehavior(rule);
		foreach (DataRow dr in drs)
		{
			string checkNumber = dr["CheckNumber"].ToString();
			dr["CheckNumber"] = padBehavior.Process(checkNumber);
		}
	}

	public void PaddingCheckNumber(DataTable dt)
	{
		foreach (PaddingRuleModel rule in RuleList)
		{
			PaddingCheckNumberByPayerId(dt, rule);
		}
	}
}

public interface IPaddingCharacterBehavior
{
	int Length { get; }
	char Character { get; }
	string Process(string checkNumber);
}

public class PaddingLeftCharacterBehavior : IPaddingCharacterBehavior
{
	public int Length { get; }
	public char Character { get; }

	public PaddingLeftCharacterBehavior(int length, char character)
	{
		this.Length = length;
		this.Character = character;
	}

	public string Process(string checkNumber)
	{
		if (checkNumber.Length < Length)
		{
			return checkNumber.PadLeft(Length, Character);
		}
		return checkNumber;
	}
}

public class PaddingRightCharacterBehavior : IPaddingCharacterBehavior
{
	public int Length { get; }
	public char Character { get; }

	public PaddingRightCharacterBehavior(int length, char character)
	{
		this.Length = length;
		this.Character = character;
	}

	public string Process(string checkNumber)
	{
		if (checkNumber.Length < Length)
		{
			return checkNumber.PadRight(Length, Character);
		}
		return checkNumber;
	}
}