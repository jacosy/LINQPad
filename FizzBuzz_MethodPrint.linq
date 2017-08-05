<Query Kind="Program" />

void Main()
{
	for (int num = 1; num <= 100; num++)
	{
		Print(num);
	}
}

// Define other methods and classes here
void Print(int num)
{
	string result = string.Empty;
	if (!PrintFizzBuzz(num, out result))
	{
		if (!PrintFizz(num, out result))
		{
			PrintBuzz(num, out result);
		}
	}
	Console.WriteLine(result);
}

bool PrintFizzBuzz(int num, out string rtnStr)
{
	bool result = false;
	rtnStr = num.ToString();
	if (num % 3 == 0 && num % 5 == 0)
	{
		result = true;
		rtnStr = "FizzBuzz";
	}
	return result;
}

bool PrintFizz(int num, out string rtnStr)
{
	bool result = false;
	rtnStr = num.ToString();
	if (num % 3 == 0)
	{
		result = true;
		rtnStr = "Fizz";
	}
	return result;
}

bool PrintBuzz(int num, out string rtnStr)
{
	bool result = false;
	rtnStr = num.ToString();
	if (num % 5 == 0)
	{
		result = true;
		rtnStr = "Buzz";
	}
	return result;
}