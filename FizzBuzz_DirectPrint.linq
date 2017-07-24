<Query Kind="Program" />

void Main()
{
	for (int num = 1; num <= 100; num++)
	{
		string result = num.ToString();
		if (num % 3 == 0 && num % 5 == 0)
		{
			result = "FizzBuzz";
		}
		else if (num % 3 == 0)
		{
			result = "Fizz";
		}
		else if (num % 5 == 0)
		{
			result = "Buzz";
		}
		Console.WriteLine(result);
	}
}

// Define other methods and classes here
