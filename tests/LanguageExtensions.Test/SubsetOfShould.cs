using Shouldly;
using Xunit;

namespace LanguageExtensions.Test;

public class SubsetOfShould
{
	public class Age : SubsetOf<int, Age>
	{
		public int Value { get; }
		private Age(int value) => Value = value;

		public static implicit operator int(Age age) => age.Value;
		public static Result<Age> Parse(int value)
			=> value switch
			{
				< 0 => new NegativeAge(),
				> 120 => new TooOld(),
				_ => new Age(value)
			};

		public record NegativeAge() : Error("Age cannot be negative");
		public record TooOld() : Error("Age cannot exceed 120");
	}

	[Fact]
	public void ReturnErrorWhenInvalid()
	{
		Result<Age> ageResult = Age.Parse(-5);
		ageResult.ShouldBeOfType<Failed<Age>>();
	}
}
