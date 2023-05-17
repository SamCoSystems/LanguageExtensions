using LanguageExtensions.Unions;
using Shouldly;
using Xunit;

namespace LanguageExtensions.Test;

public class Union2Should
{
	[Fact]
	public void SwitchCorrectly()
	{
		Union<string, int> stringOrInt = "Hello World!";
		bool isString = stringOrInt.Switch(
			(string str) => true,
			(int i) => false);
		isString.ShouldBeTrue();
	}

	[Fact]
	public void MatchIsCorrectly()
	{
		Union<string, int> stringOrInt = "Hello World!";
		bool inBlock = false;

		if (stringOrInt.Is(out string? str))
		{
			inBlock = true;
			_ = str.ShouldNotBeNull();
		}

		inBlock.ShouldBeTrue();
	}
}
