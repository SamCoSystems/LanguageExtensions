using System.Diagnostics.CodeAnalysis;

namespace LanguageExtensions;

public class Failed<Data> : Result<Data>
	where Data : notnull
{
	public Error Error { get; }
	internal Failed(Error error) => Error = error;

	public override Output Switch<Output>(
		Func<Data, Output> Success,
		Func<Error, Output> Failure)
		=> Failure(Error);

	public override Result<Output> Select<Output>(
		Func<Data, Output> selector)
		=> Error;

	public override bool UnwrapFails(
		[NotNullWhen(false)] out Data? data,
		[NotNullWhen(true)] out Error? error)
	{
		data = default;
		error = Error;
		return true;
	}
}
