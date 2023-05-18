using Xunit;
using System.Text.Json;
using LanguageExtensions.Text.Json;
using Shouldly;

namespace LanguageExtensions.Test.Text.Json;
public class MaybeConverterShould
{
	public record ComplexObject(
		Maybe<string> Name);

	private readonly JsonSerializerOptions _options;

	public MaybeConverterShould()
	{
		_options = new JsonSerializerOptions();
		_options.Converters.Add(new MaybeConverterFactory());
	}

	[Fact]
	public void SerializeSomeDataCorrectly()
	{
		string name = "John Doe";
		ComplexObject complex = new(name);
		string json = JsonSerializer.Serialize(complex, _options);
		ComplexObject? deserialized = JsonSerializer.Deserialize<ComplexObject>(json, _options);

		deserialized.ShouldNotBeNull()
			.Name.ShouldBeOfType<Some<string>>()
			.Value.ShouldBe(name);
	}

	[Fact]
	public void SerializeNoDataCorrectly()
	{
		ComplexObject complex = new(No<string>.Value);
		string json = JsonSerializer.Serialize(complex, _options);
		ComplexObject? deserialized = JsonSerializer.Deserialize<ComplexObject>(json, _options);

		_ = deserialized.ShouldNotBeNull()
			.Name.ShouldBeOfType<No<string>>();
	}
}
