namespace LanguageExtensions;

public interface ValueOf<Primitive, StrongType>
	where StrongType : notnull, ValueOf<Primitive, StrongType>
{
	public static abstract implicit operator Primitive(StrongType strongType);
	public static abstract implicit operator StrongType(Primitive primitive);
}
