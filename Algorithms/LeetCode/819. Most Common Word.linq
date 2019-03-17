<Query Kind="Program" />

void Main()
{
	//string input = "Bob hit a ball, the hit BALL flew far after it was hit.";
	//var banned = new string[] { "hit" };

	//string input = "Bob";
	//var banned = new string[] {};

	string input = "Bob. hIt, baLl";
	var banned = new string[] { "bob", "hit" };

	var sln = new Solution();
	sln.MostCommonWord(input, banned).Dump();
}

// Define other methods and classes here
public class Solution
{
	public string MostCommonWord(string paragraph, string[] banned)
	{
		var bannedWords = new HashSet<string>(banned);
		var dict = new Dictionary<string, int>();
		int wordLen = 0;

		for (int i = 0; i < paragraph.Length; i++)
		{
			int charNum = paragraph[i] - 0;
			if (charNum >= 65 && charNum <= 122)
			{
				wordLen += 1;
			}
			else
			{				
				if(wordLen!=0)
				{
					Helper(paragraph, i, ref wordLen, bannedWords, dict);
				}				
			}
		}
		
		if(wordLen!=0)
		{
			Helper(paragraph, paragraph.Length, ref wordLen, bannedWords, dict);
		}
		
		return dict.OrderByDescending(kp=> kp.Value).FirstOrDefault().Key;
	}
	
	private void Helper(string paragraph, int starLen, ref int wordLen, HashSet<string> banned,  Dictionary<string, int> dict)
	{
		string word = paragraph.Substring(starLen - wordLen, wordLen).ToLower();
		if(banned == null || banned.Count == 0)
		{
			dict[word] = dict.ContainsKey(word) ? dict[word] += 1 : 1;
		}
		else
		{
			if (!banned.Contains(word))
			{
				dict[word] = dict.ContainsKey(word) ? dict[word] += 1 : 1;
			}
		}
		
		wordLen = 0;
	}
}