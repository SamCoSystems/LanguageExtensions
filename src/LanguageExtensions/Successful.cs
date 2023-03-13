namespace LanguageExtensions;

public class Successful<Data> : Result<Data>
	where Data : notnull
{
	public Data Value { get; }
	internal Successful(Data value) => Value = value;

	public override Output Switch<Output>(
		Func<Data, Output> Success,
		Func<Error, Output> Failure)
		=> Success(Value);

	public override Result<Output> Select<Output>(
		Func<Data, Output> selector)
		=> selector(Value);
}
