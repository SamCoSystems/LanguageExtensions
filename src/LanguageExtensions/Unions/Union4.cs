using System.Diagnostics.CodeAnalysis;

namespace LanguageExtensions.Unions;

public abstract record Union<One, Two, Three, Four>
	where One : notnull
	where Two : notnull
	where Three : notnull
	where Four : notnull
{
	public static implicit operator Union<One, Two, Three, Four>(One value) => new TypeOne(value);
	public static implicit operator Union<One, Two, Three, Four>(Two value) => new TypeTwo(value);
	public static implicit operator Union<One, Two, Three, Four>(Three value) => new TypeThree(value);
	public static implicit operator Union<One, Two, Three, Four>(Four value) => new TypeFour(value);

	public abstract Output Switch<Output>(
		Func<One, Output> whenOne,
		Func<Two, Output> whenTwo,
		Func<Three, Output> whenThree,
		Func<Four, Output> whenFour);

	public abstract bool Is<Type>(
		[NotNullWhen(true)]
		out Type? value)
		where Type : class;

	public abstract bool Is<Type>(
		[NotNullWhen(true)]
		out Type? value)
		where Type : struct;

	internal record TypeOne(One Value) : Union<One, Two, Three, Four>
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
			Func<Two, Output> whenTwo,
			Func<Three, Output> whenThree,
			Func<Four, Output> whenFour)
			=> whenOne(Value);
	}

	internal record TypeTwo(Two Value) : Union<One, Two, Three, Four>
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
			Func<Two, Output> whenTwo,
			Func<Three, Output> whenThree,
			Func<Four, Output> whenFour)
			=> whenTwo(Value);
	}

	internal record TypeThree(Three Value) : Union<One, Two, Three, Four>
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
			Func<Two, Output> whenTwo,
			Func<Three, Output> whenThree,
			Func<Four, Output> whenFour)
			=> whenThree(Value);
	}

	internal record TypeFour(Four Value) : Union<One, Two, Three, Four>
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
			Func<Two, Output> whenTwo,
			Func<Three, Output> whenThree,
			Func<Four, Output> whenFour)
			=> whenFour(Value);
	}
}
