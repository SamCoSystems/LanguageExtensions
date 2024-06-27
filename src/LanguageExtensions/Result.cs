using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace LanguageExtensions;

public abstract class Result<Data>
	where Data : notnull
{
	public abstract Output Switch<Output>(
		Func<Data, Output> Success,
		Func<Error, Output> Failure);

	public abstract Result<Output> Select<Output>(
		Func<Data, Output> selector)
		where Output : notnull;

	public abstract bool UnwrapFails(
		[NotNullWhen(false)] out Data? data,
		[NotNullWhen(true)] out Error? error);

	public static implicit operator Result<Data>(Data data) => new Successful<Data>(data);
	public static implicit operator Result<Data>(Error error) => new Failed<Data>(error);

}

public static class ResultExtensions
{
	public static TaskAwaiter<Result<Data>> GetAwaiter<Data>(
		this Result<Task<Data>> resultOfTask)
		where Data : notnull
		=> resultOfTask
			.Switch(
				Success: async data => (Result<Data>)(await data),
				Failure: error => Task.FromResult((Result<Data>)error))
			.GetAwaiter();

	public static Task<Result<Output>> Select<Input, Output>(
		this Result<Input> result,
		Func<Input, Task<Output>> asyncSelector)
		where Input : notnull
		where Output : notnull
		=> result.Switch(
			Success: async data => (Result<Output>)await asyncSelector(data),
			Failure: error => Task.FromResult((Result<Output>)error));
}
