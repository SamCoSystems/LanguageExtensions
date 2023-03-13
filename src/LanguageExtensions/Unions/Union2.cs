using System.Diagnostics.CodeAnalysis;

namespace LanguageExtensions.Unions;

public abstract record Union<One, Two>
	where One : notnull
	where Two : notnull
{
	public static implicit operator Union<One, Two>(One value) => new TypeOne(value);
	public static implicit operator Union<One, Two>(Two value) => new TypeTwo(value);

	public abstract Output Switch<Output>(
		Func<One, Output> whenOne,
		Func<Two, Output> whenTwo);

	public abstract bool Is<Type>(
		[NotNullWhen(true)]
		out Type? value)
		where Type : class;

	public abstract bool Is<Type>(
		[NotNullWhen(true)]
		out Type? value)
		where Type : struct;

	internal record TypeOne(One Value) : Union<One, Two>
	{
		public override bool Is<Type>(
			[NotNullWhen(true)]
			out Type? value)
			where Type : class
		{
			if (Value is Type typedValue)
			{
				value = typedValue;
				return true;
			}
			value = null;
			return false;
		}

		public override bool Is<Type>(
			[NotNullWhen(true)]
			out Type? value)
			where Type : struct
		{
			if (Value is Type typedValue)
			{
				value = typedValue;
				return true;
			}
			value = null;
			return false;
		}

		public override Output Switch<Output>(
			Func<One, Output> whenOne,
			Func<Two, Output> whenTwo)
			=> whenOne(Value);
	}

	internal record TypeTwo(Two Value) : Union<One, Two>
	{
		public override bool Is<Type>(
			[NotNullWhen(true)]
			out Type? value)
			where Type : class
		{
			if (Value is Type typedValue)
			{
				value = typedValue;
				return true;
			}
			value = null;
			return false;
		}

		public override bool Is<Type>(
			[NotNullWhen(true)]
			out Type? value)
			where Type : struct
		{
			if (Value is Type typedValue)
			{
				value = typedValue;
				return true;
			}
			value = null;
			return false;
		}

		public override Output Switch<Output>(
			Func<One, Output> whenOne,
			Func<Two, Output> whenTwo)
			=> whenTwo(Value);
	}
}
