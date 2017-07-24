<Query Kind="Program" />

void Main()
{
	var postQcTrimBr = new BusinessRuleEOBLockboxPostQCCheckNumberTrimming();
	// get data
	var mock = new MockObjects();
	DataTable dt = mock.GetData();
	// Process data base the formats
	postQcTrimBr.ProcessCheckNumber(dt, "19046,896D|27154,%**|LBW3CO,***%||test,");
	//postQcTrimBr.ProcessCheckNumber(dt, "");
	dt.Dump();
}

public class MockObjects
{
	public DataTable GetData()
	{
		DataTable dt = new DataTable();
		dt.Columns.Add("PayerID");
		dt.Columns.Add("CheckNumber");

		dt.Rows.Add("19046", "896D111111");
		dt.Rows.Add("19046", "222222896D");
		dt.Rows.Add("19046", "896D333333896D");
		dt.Rows.Add("19046", "444444");
		dt.Rows.Add("27154", "2T5555");
		dt.Rows.Add("27154", "AB6666");
		dt.Rows.Add("27154", "S");
		dt.Rows.Add("27154", "st");
		dt.Rows.Add("LBW3CO", "7777CCC");
		dt.Rows.Add("LBW3CO", "88888XXX");
		dt.Rows.Add("LBW3CO", "11");
		dt.Rows.Add("LBW3CO", "111");
		return dt;
	}
}

public class BusinessRuleEOBLockboxPostQCCheckNumberTrimming
{
	// Transfer format string to dictionary
	Dictionary<string, string> TransferCheckNumberFormatToDictionary(string checkNumberFormat)
	{
		var formatDic = new Dictionary<string, string>();

		var formats = checkNumberFormat.Split('|');
		foreach (string f in formats)
		{
			var format = f.Split(',');
			if (format.Length != 2)
			{
				//throw new FormatException("PostQCCheckNumberTrimCheckNumberFormat is not correct!");
			}
			else
			{
				formatDic.Add(format[0], format[1]);
			}
		}

		return formatDic;
	}

	void ProcessCheckNumberByPayerId(DataRow[] drs, string characters)
	{
		string newCharacters = string.Empty;
		IProcessCheckNumberFactory checkNumberFactory = new ProcessCheckNumberFactory();
		IProcessCheckNumberBehavior checkNumberBehavior = checkNumberFactory.GetProcessCheckNumberBehavior(characters);

		if (checkNumberBehavior != null)
		{
			foreach (var dr in drs)
			{
				dr["CheckNumber"] = checkNumberBehavior.Process(dr["CheckNumber"].ToString());
			}
		}
	}

	public void ProcessCheckNumber(DataTable dt, string checkNumberFormat)
	{
		Dictionary<string, string> formatDic = TransferCheckNumberFormatToDictionary(checkNumberFormat);
		string newCharacters = string.Empty;

		foreach (var element in formatDic)
		{
			ProcessCheckNumberByPayerId(dt.Select(string.Format("PayerID = '{0}'", element.Key)), element.Value);
		}
	}
}

// Define other methods and classes here
public enum ProcessCheckNumberType
{
	TrimStart,
	TrimEnd,
	Replace,
	None
}

public interface IProcessCheckNumberBehavior
{
	string Characters { get; }
	string Process(string checkNumber);
}

public class TrimStartCheckNumberByNumberOfStartSigns : IProcessCheckNumberBehavior
{
	public string Characters { get; private set; }

	public TrimStartCheckNumberByNumberOfStartSigns(string characters)
	{
		this.Characters = characters;
	}

	public string Process(string checkNumber)
	{
		string newCheckNumber = checkNumber;
		if (checkNumber.Length >= Characters.Length)
		{
			newCheckNumber = checkNumber.Substring(Characters.Length);
		}
		return newCheckNumber;
	}
}

public class TrimEndCheckNumberByNumberOfStartSigns : IProcessCheckNumberBehavior
{
	public string Characters { get; private set; }

	public TrimEndCheckNumberByNumberOfStartSigns(string characters)
	{
		this.Characters = characters;
	}

	public string Process(string checkNumber)
	{
		string newCheckNumber = checkNumber;
		if (checkNumber.Length >= Characters.Length)
		{
			newCheckNumber = checkNumber.Substring(0, checkNumber.Length - Characters.Length);
		}
		return newCheckNumber;
	}
}

public class RemoveCheckNumberBySpecificCharacters : IProcessCheckNumberBehavior
{
	public string Characters { get; private set; }

	public RemoveCheckNumberBySpecificCharacters(string characters)
	{
		this.Characters = characters;
	}

	public string Process(string checkNumber)
	{
		return checkNumber.Replace(Characters, "");
	}
}

public interface IProcessCheckNumberFactory
{
	IProcessCheckNumberBehavior GetProcessCheckNumberBehavior(string characters);
}

public class ProcessCheckNumberFactory : IProcessCheckNumberFactory
{
	bool CheckIsTrimCharacters(string trimCharacters)
	{
		int totaltrimCharacters = 0;
		var characterArr = trimCharacters.Replace("%", "").Split('*');
		foreach (string element in characterArr)
		{
			if (string.IsNullOrEmpty(element))
			{
				totaltrimCharacters += 1;
			}
		}
		return totaltrimCharacters == trimCharacters.Length;
	}

	ProcessCheckNumberType GetTrimType(string characters)
	{
		var type = ProcessCheckNumberType.None;
		if (CheckIsTrimCharacters(characters))
		{
			if (characters.StartsWith("%"))
			{
				type = ProcessCheckNumberType.TrimStart;
			}
			else if (characters.EndsWith("%"))
			{
				type = ProcessCheckNumberType.TrimEnd;
			}
		}
		return type;
	}

	ProcessCheckNumberType GetProcessCheckNumberType(string characters, out string newCharacters)
	{
		newCharacters = characters.Trim();
		ProcessCheckNumberType type;
		if (string.IsNullOrEmpty(newCharacters))
		{
			type = ProcessCheckNumberType.None;
		}
		else
		{
			type = GetTrimType(newCharacters);
			if (type == ProcessCheckNumberType.None)
			{
				type = ProcessCheckNumberType.Replace;
			}
			else
			{
				newCharacters = newCharacters.Replace("%", "");
			}
		}
		return type;
	}

	public IProcessCheckNumberBehavior GetProcessCheckNumberBehavior(string characters)
	{
		string newCharacters = string.Empty;
		ProcessCheckNumberType processType = GetProcessCheckNumberType(characters, out newCharacters);

		IProcessCheckNumberBehavior behavior = null;
		switch (processType)
		{
			case ProcessCheckNumberType.TrimStart:
				behavior = new TrimStartCheckNumberByNumberOfStartSigns(newCharacters);
				break;
			case ProcessCheckNumberType.TrimEnd:
				behavior = new TrimEndCheckNumberByNumberOfStartSigns(newCharacters);
				break;
			case ProcessCheckNumberType.Replace:
				behavior = new RemoveCheckNumberBySpecificCharacters(newCharacters);
				break;
		}
		return behavior;
	}
}