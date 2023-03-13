using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LanguageExtensions.Test;

public class MaybeShould
{
	[Fact]
	public void BeAssignableFromType()
	{
		Maybe<string> maybeString = "Hello World!";
	}

	[Fact]
	public void BeAssignableFromNoDataValue()
	{
		Maybe<string> maybeString = No<string>.Value;
	}

	[Fact]
	public void SwitchToSomeWhenSome()
	{
		Maybe<string> maybeString = "Hello World!";
		bool isString = maybeString.Switch(
			Some: str => true,
			None: () => false);

		isString.ShouldBeTrue();
	}

	[Fact]
	public void SwitchToNoneWhenNoValue()
	{
		Maybe<string> maybeString = No<string>.Value;
		bool isString = maybeString.Switch(
			Some: str => true,
			None: () => false);

		isString.ShouldBeFalse();
	}

	[Fact]
	public void SelectWhenSome()
	{
		Maybe<string> maybeString = "Hello World!";
		Func<string, int> length = str => str.Length;
		Maybe<int> maybeInt = maybeString.Select(length);

		maybeInt.ShouldBeOfType<Some<int>>();
	}

	[Fact]
	public void ReturnNoValueWhenNoValue()
	{
		Maybe<string> maybeString = No<string>.Value;
		Func<string, int> length = str => str.Length;
		Maybe<int> maybeInt = maybeString.Select(length);

		maybeInt.ShouldBe(No<int>.Value);
	}

	[Fact]
	public void NotCallSelectorWhenNoValue()
	{
		Maybe<string> maybeString = No<string>.Value;
		bool called = false;
		Func<string, int> length = str =>
		{
			called = true;
			return str.Length;
		};
		Maybe<int> maybeInt = maybeString.Select(length);

		called.ShouldBeFalse();
	}

	[Fact]
	public async Task ShouldBeAwaitableIfMaybeOfTask()
	{
		Maybe<Task<int>> maybeTask = Task.FromResult(1);
		var maybeInt = await maybeTask;
		maybeInt.ShouldBeAssignableTo<Maybe<int>>();
	}

	[Fact]
	public void ShouldHaveOnlyOneTaskAtTheOuterMostLayer()
	{
		Func<int, Task<int>> add5Async = x => Task.FromResult(x + 5);
		Maybe<int> maybeInt = 0;
		var maybeIntPlus5 = maybeInt.Select(add5Async);
		maybeIntPlus5.ShouldBeAssignableTo<Task<Maybe<int>>>();
	}

	[Fact]
	public void ShouldBeAbleToSelectTasksOfMaybesAndGetTaskOfMaybes()
	{
		Func<int, Task<int>> add5Async = x => Task.FromResult(x + 5);
		Maybe<int> maybeInt = 0;
		var maybeIntPlus5 = maybeInt.Select(add5Async);
		var maybeIntPlus10 = maybeIntPlus5.Select(add5Async);
		maybeIntPlus10.ShouldBeAssignableTo<Task<Maybe<int>>>();
	}

	[Fact]
	public void ShouldBeAbleToMixSyncAndAsyncWhenSelecting()
	{
		Func<int, Task<int>> add5Async = x => Task.FromResult(x + 5);
		Func<int, int> add5 = x => x + 5;
		Maybe<int> maybeInt = 0;
		var maybeIntPlus5 = maybeInt.Select(add5Async);
		var maybeIntPlus10 = maybeIntPlus5.Select(add5);
		var maybeIntPlus15 = maybeIntPlus10.Select(add5Async);
		var maybeIntPlus20 = maybeIntPlus15.Select(add5);
		maybeIntPlus20.ShouldBeAssignableTo<Task<Maybe<int>>>();
	}
	
	[Fact]
	public void ShouldSelectToAtMostOneTaskWrappingOneMaybe()
	{
		Func<int, Task<int>> add5Async = x => Task.FromResult(x + 5);
		Func<int, Maybe<int>> add5Maybe = x => x + 5;
		Func<int, Task<Maybe<int>>> add5MaybeAsync = x => Task.FromResult<Maybe<int>>(x + 5);
		Maybe<int> maybeInt = 0;
		var result = maybeInt
			.Select(add5Maybe)
			.Select(add5Async)
			.Select(add5MaybeAsync)
			.Select(add5Async)
			.Select(add5Maybe);

		result.ShouldBeAssignableTo<Task<Maybe<int>>>();
	}
}
