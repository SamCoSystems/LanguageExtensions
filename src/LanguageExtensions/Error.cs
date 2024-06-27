using System;

namespace LanguageExtensions;

public abstract record Error(string Message);
public record CaughtException(Exception Exception)
	: Error($"Caught: {Exception.GetType().Name} {Exception.Message}");

public static class Safely
{
	public static Result<Data> Execute<Data>(Func<Data> func)
		where Data : notnull
	{
		try
		{
			return func();
		}
		catch (Exception exception)
		{
			return new CaughtException(exception);
		}
	}
}