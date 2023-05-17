namespace LanguageExtensions;

public class No<Data> : Maybe<Data>
	where Data : notnull
{
	private No() { }
	public static Maybe<Data> Value { get; } = new No<Data>();

	public override Output Switch<Output>(
		Func<Data, Output> Some,
		Func<Output> None)
		=> None();

	public override Maybe<Output> Select<Output>(
		Func<Data, Output> selector)
		=> No<Output>.Value;

	public override Maybe<Output> Select<Output>(
		Func<Data, Maybe<Output>> selector)
		=> No<Output>.Value;

	public override Task<Maybe<Output>> Select<Output>(
		Func<Data, Task<Output>> selector)
		=> Task.FromResult(No<Output>.Value);

	public override Task<Maybe<Output>> Select<Output>(
		Func<Data, Task<Maybe<Output>>> selector)
		=> Task.FromResult(No<Output>.Value);
}
