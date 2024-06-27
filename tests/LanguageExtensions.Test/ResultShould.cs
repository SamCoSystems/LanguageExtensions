using Shouldly;
using System.Threading.Tasks;
using System;
using Xunit;
using System.Collections.Generic;

namespace LanguageExtensions.Test;

public record AnonymousError : Error
{
	public AnonymousError() : base("Anonymous Error") { }
}

public class ResultShould
{
	[Fact]
	public void BeAssignableFromType()
	{
		Result<string> maybeString = "Hello World!";
	}

	[Fact]
	public void BeAssignableFromError()
	{
		Result<string> maybeString = new AnonymousError();
	}

	[Fact]
	public void SwitchToSuccessfulWhenSuccessful()
	{
		Result<string> resultofString = "Hello World!";
		bool isString = resultofString.Switch(
			Success: str => true,
			Failure: error => false);

		isString.ShouldBeTrue();
	}

	[Fact]
	public void SwitchToNoneWhenNoValue()
	{
		Result<string> resultofString = new AnonymousError();
		bool isString = resultofString.Switch(
			Success: str => true,
			Failure: error => false);

		isString.ShouldBeFalse();
	}

	[Fact]
	public void SelectWhenSuccessful()
	{
		Result<string> resultOfString = "Hello World!";
		Func<string, int> length = str => str.Length;
		Result<int> resultOfInt = resultOfString.Select(length);

		resultOfInt.ShouldBeOfType<Successful<int>>();
	}

	[Fact]
	public void ReturnErrorWhenFailure()
	{
		Result<string> resultOfString = new AnonymousError();
		Func<string, int> length = str => str.Length;
		Result<int> resultOfInt = resultOfString.Select(length);

		resultOfInt.ShouldBeOfType<Failed<int>>();
	}

	[Fact]
	public void NotCallSelectorWhenError()
	{
		Result<string> resultOfString = new AnonymousError();
		bool called = false;
		Func<string, int> length = str =>
		{
			called = true;
			return str.Length;
		};
		Result<int> resultOfInt = resultOfString.Select(length);

		called.ShouldBeFalse();
	}

	[Fact]
	public async Task BeAwaitableIfResultOfTask()
	{
		Result<Task<int>> resultOfTask = Task.FromResult(1);
		var resultOfInt = await resultOfTask;
		resultOfInt.ShouldBeAssignableTo<Result<int>>();
	}

	[Fact]
	public void UnwrapOrEarlyReturn()
	{
		Result<string> helloR = "hello";
		Result<int> fiveR = 5;
		Result<string> hellosR = AttemptEarlyReturn(helloR, fiveR);
		Successful<string> hellosS = hellosR.ShouldBeOfType<Successful<string>>();
		hellosS.Value.ShouldBe("5 hellos to you!");
	}

	private Result<string> AttemptEarlyReturn(
		Result<string> helloR,
		Result<int> fiveR)
	{
		if (helloR.UnwrapFails(out var hello, out var error)) return error;
		if (fiveR.UnwrapFails(out var five, out error)) return error;

		return $"{five} {hello}s to you!";
	}

	/*
	[Fact]
	public void ShouldHaveOnlyOneTaskAtTheOuterMostLayer()
	{
		Func<int, Task<int>> add5async = x => Task.FromResult(x+5);
		Result<int> resultOfInt = 0;
		var resultIntPlus5 = resultOfInt.Select(add5async);
		resultIntPlus5.ShouldBeAssignableTo<Task<Result<int>>>();
	}

	/*
	[Fact]
	public void ShouldHaveOnlyOneTaskAtTheOuterMostLayer()
	{
		Func<int, Task<int>> add5Async = x => Task.FromResult(x + 5);
		Result<int> maybeInt = 0;
		var maybeIntPlus5 = maybeInt.Select(add5Async);
		maybeIntPlus5.ShouldBeAssignableTo<Task<Result<int>>>();
	}

	[Fact]
	public void ShouldBeAbleToSelectTasksOfMaybesAndGetTaskOfMaybes()
	{
		Func<int, Task<int>> add5Async = x => Task.FromResult(x + 5);
		Result<int> maybeInt = 0;
		var maybeIntPlus5 = maybeInt.Select(add5Async);
		var maybeIntPlus10 = maybeIntPlus5.Select(add5Async);
		maybeIntPlus10.ShouldBeAssignableTo<Task<Result<int>>>();
	}

	[Fact]
	public void ShouldBeAbleToMixSyncAndAsyncWhenSelecting()
	{
		Func<int, Task<int>> add5Async = x => Task.FromResult(x + 5);
		Func<int, int> add5 = x => x + 5;
		Result<int> maybeInt = 0;
		var maybeIntPlus5 = maybeInt.Select(add5Async);
		var maybeIntPlus10 = maybeIntPlus5.Select(add5);
		var maybeIntPlus15 = maybeIntPlus10.Select(add5Async);
		var maybeIntPlus20 = maybeIntPlus15.Select(add5);
		maybeIntPlus20.ShouldBeAssignableTo<Task<Result<int>>>();
	}

	[Fact]
	public void ShouldSelectToAtMostOneTaskWrappingOneMaybe()
	{
		Func<int, Task<int>> add5Async = x => Task.FromResult(x + 5);
		Func<int, Result<int>> add5Maybe = x => x + 5;
		Func<int, Task<Result<int>>> add5MaybeAsync = x => Task.FromResult<Result<int>>(x + 5);
		Result<int> maybeInt = 0;
		var result = maybeInt
			.Select(add5Maybe)
			.Select(add5Async)
			.Select(add5MaybeAsync)
			.Select(add5Async)
			.Select(add5Maybe);

		result.ShouldBeAssignableTo<Task<Result<int>>>();
	}
	*/
}
