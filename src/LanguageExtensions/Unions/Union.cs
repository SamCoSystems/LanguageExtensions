using System.Diagnostics.CodeAnalysis;

namespace LanguageExtensions.Unions;

internal static class Union
{
	public static bool Is<Type>(
		object innerValue,
		[NotNullWhen(true)]
		out Type? value)
		where Type : struct
	{
		if (innerValue is Type typedValue)
		{
			value = typedValue;
			return true;
		}

		value = null;
		return false;
	}

	public static bool Is<Type>(
		object innerValue,
		[NotNullWhen(true)]
		out Type? value)
		where Type : class
	{
		if (innerValue is Type typedValue)
		{
			value = typedValue;
			return true;
		}

		value = null;
		return false;
	}
}
