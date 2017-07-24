<Query Kind="Program" />

void Main()
{
	//SubqueryLambdaExpression();
	//SubqueryLambdaExpression();
	//ProgressiveQueryExpression();
	//IntoQueryExpression();
	WrappingQueryExpression();
}

// Define other methods and classes here
public void SubqueryLambdaExpression()
{
	IEnumerable<string> names = Namelist();
	IEnumerable<string> outerQuery = Namelist().Where(n =>
	  	n.Length == names.OrderBy(n2 => n2.Length)
						 .Select(n2 => n2.Length).First());

	outerQuery.Dump("Subquery Lambda Expression");
}

public void SubqueryQueryExpression()
{
	IEnumerable<string> names = Namelist();
	var result = from n in names
				 where n.Length == (from n2 in names orderby n2.Length select n2.Length).First()
				 select n;
	names.Dump("Subquery Query Expression");
}

public void ProgressiveQueryExpression()
{
	IEnumerable<string> names = Namelist();
	// first section
	IEnumerable<string> query = from n in names
										select n.Replace("a", "").Replace("e", "").Replace("i", "")
												 .Replace("o", "").Replace("u", "");
	// second section											 
	query = from n in query where n.Length > 2 orderby n select n;
	query.Dump("Progressive Query Expression");
}

public void IntoQueryExpression()
{
	IEnumerable<string> names = Namelist();
	
	IEnumerable<string> query = from n in names
								select n.Replace("a", "").Replace("e", "").Replace("i", "")
										 .Replace("o", "").Replace("u", "")
								into noVowel
								where noVowel.Length > 2
								orderby noVowel
								select noVowel;
								
	query.Dump("Into Query Expression");
}

public void WrappingQueryExpression()
{
	IEnumerable<string> names = Namelist();

	IEnumerable<string> query = from n in
									(from n in names
									 select n.Replace("a", "").Replace("e", "").Replace("i", "")
											 .Replace("o", "").Replace("u", ""))
								where n.Length > 2
								orderby n
								select n;

	query.Dump("Wrapping Query Expression");
}

public IEnumerable<string> Namelist()
{
	return new string[] { "Tom", "Dick", "Harry", "Mary", "Jay" };
}