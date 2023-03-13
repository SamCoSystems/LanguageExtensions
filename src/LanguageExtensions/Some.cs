namespace LanguageExtensions;

public class Some<Data> : Maybe<Data>
	where Data : notnull
{
	public Data Value { get; }
	internal Some(Data data) => Value = data;

	public override Output Switch<Output>(
		Func<Data, Output> Some,
		Func<Output> None)
		=> Some(Value);

	public override Maybe<Output> Select<Output>(
		Func<Data, Output> selector)
		=> selector(Value);

	public override Maybe<Output> Select<Output>(
		Func<Data, Maybe<Output>> selector)
		=> selector(Value);

	public override async Task<Maybe<Output>> Select<Output>(
		Func<Data, Task<Output>> selector)
		=> await selector(Value);

	public override async Task<Maybe<Output>> Select<Output>(
		Func<Data, Task<Maybe<Output>>> selector)
		=> await selector(Value);
}
