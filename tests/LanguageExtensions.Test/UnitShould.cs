using Shouldly;
using Xunit;

namespace LanguageExtensions.Test;
public class UnitShould
{
	[Fact]
	public void ReturnSingleInstanceFromStaticAccessor()
	{
		var unit = Unit.Value;
		var otherUnit = Unit.Value;
		var sameInstance = ReferenceEquals(unit, otherUnit);
		sameInstance.ShouldBeTrue();
	}
}