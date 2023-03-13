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
				< 0 => Negative(),
				> 120 => TooOld(),
				_ => new Age(value)
			};

		private static Error Negative()
			=> new("AgeError-01", "Age cannot be negative");

		private static Error TooOld()
			=> new("AgeError-02", "Age cannot exceed 120");
	}

	[Fact]
	public void ReturnErrorWhenInvalid()
	{
		Result<Age> ageResult = Age.Parse(-5);
		ageResult.ShouldBeOfType<Failed<Age>>();
	}
}
