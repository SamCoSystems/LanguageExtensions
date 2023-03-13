using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

	public static implicit operator Result<Data>(Data data) => new Successful<Data>(data);
	public static implicit operator Result<Data>(Error error) => new Failed<Data>(error);
}

public static class ResultExtensions
{
	public static Data Unwrap<Data>(
		this Result<Data> result)
		where Data : notnull
		=> result.Switch(
			value => value,
			error => throw new Exception(error.Message));

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
