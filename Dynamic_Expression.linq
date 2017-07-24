<Query Kind="Program" />

void Main()
{
	
}

public static class DynamicExtensions
{
	private static Expression<Func<T, object>> BuilderSortExpression<T>(string field)
	{
		var parameter = Expression.Parameter(typeof(T), "item");
		var expression = Expression.Lambda<Func<T, object>>(Expression.Property(parameter, field), parameter);
		return expression;
	}
}