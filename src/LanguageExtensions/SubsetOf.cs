namespace LanguageExtensions;

public interface SubsetOf<SuperType, SubType>
	where SubType : notnull, SubsetOf<SuperType, SubType>
{
	public static abstract implicit operator SuperType(SubType subType);
	public static abstract Result<SubType> Parse(SuperType super);
}
