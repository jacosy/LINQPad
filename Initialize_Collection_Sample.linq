<Query Kind="Program" />

void Main()
{
	// Initialize IEnumerable<T> with some items
	IEnumerable<Movie> movies = new[]
	{
		new Movie {Id=1,Name="Star Wars"},
		new Movie {Id=2,Name="The Weather Man"},
		new Movie {Id=3,Name="The Martian"}
	};
	movies.Dump();

	// IEnumerable<T> can't use this way!
	var movies2 = new List<Movie>
	{
		new Movie {Id=4,Name="Star Wars 2"},
		new Movie {Id=5,Name="Shame"},
		new Movie {Id=6,Name="The Accountant"}
	};
	movies2.Dump();
}

// Define other methods and classes here
public class Movie
{
	public int Id { get; set; }
	public string Name { get; set; }
}