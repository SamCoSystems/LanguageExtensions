using System.Diagnostics.CodeAnalysis;

namespace LanguageExtensions.Unions;

public abstract record Union<One, Two, Three>
	where One : notnull
	where Two : notnull
	where Three : notnull
{
	public static implicit operator Union<One, Two, Three>(One value) => new TypeOne(value);
	public static implicit operator Union<One, Two, Three>(Two value) => new TypeTwo(value);
	public static implicit operator Union<One, Two, Three>(Three value) => new TypeThree(value);

	public abstract Output Switch<Output>(
		Func<One, Output> whenOne,
		Func<Two, Output> whenTwo,
		Func<Three, Output> whenThree);

	public abstract bool Is<Type>(
		[NotNullWhen(true)]
		out Type? value)
		where Type : class;

	public abstract bool Is<Type>(
		[NotNullWhen(true)]
		out Type? value)
		where Type : struct;

	internal record TypeOne(One Value) : Union<One, Two, Three>
	{
		public override bool Is<Type>(
			[NotNullWhen(true)]
			out Type? value)
			where Type : class
			=> Union.Is(Value, out value);

		public override bool Is<Type>(
			[NotNullWhen(true)]
			out Type? value)
			where Type : struct
			=> Union.Is(Value, out value);

		public override Output Switch<Output>(
			Func<One, Output> whenOne,
			Func<Two, Output> whenTwo,
			Func<Three, Output> whenThree)
			=> whenOne(Value);
	}

	internal record TypeTwo(Two Value) : Union<One, Two, Three>
	{
		public override bool Is<Type>(
			[NotNullWhen(true)]
			out Type? value)
			where Type : class
			=> Union.Is(Value, out value);

		public override bool Is<Type>(
			[NotNullWhen(true)]
			out Type? value)
			where Type : struct
			=> Union.Is(Value, out value);

		public override Output Switch<Output>(
			Func<One, Output> whenOne,
			Func<Two, Output> whenTwo,
			Func<Three, Output> whenThree)
			=> whenTwo(Value);
	}

	internal record TypeThree(Three Value) : Union<One, Two, Three>
	{
		public override bool Is<Type>(
			[NotNullWhen(true)]
			out Type? value)
			where Type : class
			=> Union.Is(Value, out value);

		public override bool Is<Type>(
			[NotNullWhen(true)]
			out Type? value)
			where Type : struct
			=> Union.Is(Value, out value);

		public override Output Switch<Output>(
			Func<One, Output> whenOne,
			Func<Two, Output> whenTwo,
			Func<Three, Output> whenThree)
			=> whenThree(Value);
	}
}
