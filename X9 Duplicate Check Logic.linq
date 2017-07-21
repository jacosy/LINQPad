<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	List<EOBLockbox_Check> testOneCheckIdChecks = GenerateOnlyOneCheckIdData();
	updateDuplicatedCheck(testOneCheckIdChecks);
	testOneCheckIdChecks.Dump();

	List<EOBLockbox_Check> testTwoChecks = GenerateTwoCheckIdData();
	updateDuplicatedCheck(testTwoChecks);
	testTwoChecks.Dump();

	List<EOBLockbox_Check> testMultiChecks = GenerateMultiCheckIdData();
	updateDuplicatedCheck(testMultiChecks);
	testMultiChecks.Dump();
}

public List<EOBLockbox_Check> GenerateOnlyOneCheckIdData()
{
	return new List<EOBLockbox_Check> {
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4112070, duplicateClientName="UNC PA", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4112072, duplicateClientName="UNC PA", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4112079, duplicateClientName="MEDICOUNT", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4113770, duplicateClientName="UNC PA", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4113772, duplicateClientName="MEDICOUNT", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4114101, duplicateClientName="UNC PA", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4114104, duplicateClientName="MEDICOUNT", x9Error ="Test"  }
	};
}

public List<EOBLockbox_Check> GenerateTwoCheckIdData()
{
	return new List<EOBLockbox_Check> {
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4112070, duplicateClientName="UNC PA", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4112072, duplicateClientName="UNC PA", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4112079, duplicateClientName="MEDICOUNT", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4113770, duplicateClientName="UNC PA", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113809, duplicateCheckID =4113772, duplicateClientName="MEDICOUNT", x9Error ="YEAH"  },
		new EOBLockbox_Check{ checkID = 4113809, duplicateCheckID =4114101, duplicateClientName="UNC PA", x9Error ="YEAH"  },
		new EOBLockbox_Check{ checkID = 4113809, duplicateCheckID =4114104, duplicateClientName="MEDICOUNT", x9Error ="YEAH"  }
	};
}

public List<EOBLockbox_Check> GenerateMultiCheckIdData()
{
	return new List<EOBLockbox_Check> {
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4112070, duplicateClientName="UNC PA", x9Error ="Error"  },
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4112072, duplicateClientName="UNC PA", x9Error ="Error"  },
		new EOBLockbox_Check{ checkID = 4113806, duplicateCheckID =4112079, duplicateClientName="MEDICOUNT", x9Error ="Error"  },
		new EOBLockbox_Check{ checkID = 4113808, duplicateCheckID =4113770, duplicateClientName="UNC PA", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113808, duplicateCheckID =4113772, duplicateClientName="MEDICOUNT", x9Error ="Test"  },
		new EOBLockbox_Check{ checkID = 4113809, duplicateCheckID =4114101, duplicateClientName="UNC PA", x9Error ="No"  },
		new EOBLockbox_Check{ checkID = 4113807, duplicateCheckID =4114104, duplicateClientName="MEDICOUNT", x9Error ="YEAH"  }
	};
}

// Define other methods and classes here
private void updateDuplicatedCheck(List<EOBLockbox_Check> checkWithDuplicates)
{
	int totalCheck = checkWithDuplicates.Count;
	int nextCheckId = 0;
	var errorMessage = new StringBuilder();

	for (int i = 0; i < totalCheck; i++)
	{
		if (i + 1 < totalCheck)
		{
			nextCheckId = checkWithDuplicates[i + 1].checkID;
		}
		
		errorMessage.AppendLine(string.Format("The Check ID: {0} with the Client Name: {1} is a duplicate.",
			checkWithDuplicates[i].duplicateCheckID.ToString(), checkWithDuplicates[i].duplicateClientName));		
						
		if (nextCheckId != checkWithDuplicates[i].checkID || i + 1 >= totalCheck)
		{
			// Update this check
			errorMessage.Insert(0, (string.IsNullOrEmpty(checkWithDuplicates[i].x9Error) ? "" : checkWithDuplicates[i].x9Error + "\n"));
			var selectedChecks = checkWithDuplicates.Where(ck => ck.checkID == checkWithDuplicates[i].checkID);
			foreach (var check in selectedChecks)
			{
				check.x9Error = errorMessage.ToString();
			}
			errorMessage.Remove(0, errorMessage.Length);
		}
	}
}

public class EOBLockbox_Check
{
	public int checkID { get; set; }
	public int duplicateCheckID { get; set; }
	public string duplicateClientName { get; set; }
	public string x9Error { get; set; }
}