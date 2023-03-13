namespace LanguageExtensions;
public class Unit
{
	private static Unit _instance = new Unit();
	private Unit() { }
	public static Unit Value => _instance;
}