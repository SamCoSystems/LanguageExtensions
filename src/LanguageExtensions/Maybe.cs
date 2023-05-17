using System.Runtime.CompilerServices;

namespace LanguageExtensions;

public abstract class Maybe<Data>
	where Data : notnull
{
	public static implicit operator Maybe<Data>(Data data) => new Some<Data>(data);

	public abstract Output Switch<Output>(
		Func<Data, Output> Some,
		Func<Output> None);

	public abstract Maybe<Output> Select<Output>(
		Func<Data, Output> selector)
		where Output : notnull;

	public abstract Maybe<Output> Select<Output>(
		Func<Data, Maybe<Output>> selector)
		where Output: notnull;

	public abstract Task<Maybe<Output>> Select<Output>(
		Func<Data, Task<Output>> selector)
		where Output : notnull;

	public abstract Task<Maybe<Output>> Select<Output>(
		Func<Data, Task<Maybe<Output>>> selector)
		where Output : notnull;
}

public static class MaybeExtensions
{
	public static TaskAwaiter<Maybe<Data>> GetAwaiter<Data>(
		this Maybe<Task<Data>> maybeTask)
		where Data : notnull
	{
		if (maybeTask is Some<Task<Data>> someTask)
		{
			Task<Data> task = someTask.Value;
			Task<Maybe<Data>> continuation = task.ContinueWith(TaskOfDataAsMaybe);
			return continuation.GetAwaiter();
		}

		Maybe<Data> result = No<Data>.Value;
		return Task.FromResult(result).GetAwaiter();
	}

	private static Maybe<Data> TaskOfDataAsMaybe<Data>(Task<Data> taskOfData)
		where Data : notnull
		=> (Maybe<Data>)taskOfData.GetAwaiter().GetResult();

	public static async Task<Maybe<Output>> Select<Data, Output>(
		this Task<Maybe<Data>> taskOfMaybe,
		Func<Data, Output> selector)
		where Data : notnull
		where Output : notnull
		=> (await taskOfMaybe).Select(selector);

	public static async Task<Maybe<Output>> Select<Data, Output>(
		this Task<Maybe<Data>> taskOfMaybe,
		Func<Data, Maybe<Output>> selector)
		where Data : notnull
		where Output : notnull
		=> (await taskOfMaybe).Select(selector);

	public static async Task<Maybe<Output>> Select<Data, Output>(
		this Task<Maybe<Data>> taskOfMaybe,
		Func<Data, Task<Output>> selector)
		where Data : notnull
		where Output : notnull
		=>	await (await taskOfMaybe).Select(selector);

	public static async Task<Maybe<Output>> Select<Data, Output>(
		this Task<Maybe<Data>> taskOfMaybe,
		Func<Data, Task<Maybe<Output>>> selector)
		where Data : notnull
		where Output : notnull
		=> await (await taskOfMaybe).Select(selector);
}