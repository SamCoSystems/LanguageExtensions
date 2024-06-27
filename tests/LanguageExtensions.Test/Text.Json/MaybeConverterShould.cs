using Xunit;
using System.Text.Json;
using LanguageExtensions.Text.Json;
using Shouldly;

namespace LanguageExtensions.Test.Text.Json;
public class MaybeConverterShould
{
	public record ComplexStringObject(
		Maybe<string> Name);

	public record ComplexIntObject(
		Maybe<int> Age);

	public record Person(
		string Name,
		int Age,
		Maybe<string> Title);

	public record ComplexComplexObject(
		Maybe<Person> Person);

	private readonly JsonSerializerOptions _options;

	public MaybeConverterShould()
	{
		_options = new JsonSerializerOptions();
		_options.Converters.Add(new MaybeConverterFactory());
	}

	[Fact]
	public void SerializeSomeStringCorrectly()
	{
		string name = "John Doe";
		ComplexStringObject complex = new(name);
		string expectedJson = "{\"Name\":\"John Doe\"}";

		string json = JsonSerializer.Serialize(complex, _options);
		json.ShouldBe(expectedJson);

		ComplexStringObject? deserialized = JsonSerializer.Deserialize<ComplexStringObject>(json, _options);
		deserialized.ShouldNotBeNull()
			.Name.ShouldBeOfType<Some<string>>()
			.Value.ShouldBe(name);
	}

	[Fact]
	public void SerializeSomeIntCorrectly()
	{
		int age = 21;
		ComplexIntObject complex = new(age);
		string expectedJson = "{\"Age\":21}";

		string json = JsonSerializer.Serialize(complex, _options);
		json.ShouldBe(expectedJson);

		ComplexIntObject? deserialized = JsonSerializer.Deserialize<ComplexIntObject>(json, _options);
		deserialized.ShouldNotBeNull()
			.Age.ShouldBeOfType<Some<int>>()
			.Value.ShouldBe(age);
	}

	[Theory]
	[InlineData("Dr.", "\"Dr.\"")]
	[InlineData(null, "null")]
	public void SerializeSomeComplexObjectCorrect(
		string? titleInput,
		string titleJson)
	{
		string name = "John Doe";
		int age = 21;
		Maybe<string> title = titleInput ?? No<string>.Value;
		ComplexComplexObject complex = new(new Person(name, age, title));
		string expectedJson = "{\"Person\":{\"Name\":\"John Doe\",\"Age\":21,\"Title\":"+titleJson+"}}";

		string json = JsonSerializer.Serialize(complex, _options);
		json.ShouldBe(expectedJson);

		ComplexComplexObject? deserialized = JsonSerializer.Deserialize<ComplexComplexObject>(json, _options);
		Person person = deserialized.ShouldNotBeNull()
			.Person.ShouldBeOfType<Some<Person>>()
			.Value;

		person.Name.ShouldBe(name);
		person.Age.ShouldBe(age);
		title.Switch(
			Some: value =>
			{
				person.Title.ShouldBeOfType<Some<string>>().Value.ShouldBe(value);
				return Unit.Value;
			},
			None: () =>
			{
				person.Title.ShouldBeOfType<No<string>>();
				return Unit.Value;
			}
		);
	}

	[Fact]
	public void SerializeNoDataCorrectly()
	{
		ComplexStringObject complex = new(No<string>.Value);
		string expectedJson = "{\"Name\":null}";

		string json = JsonSerializer.Serialize(complex, _options);
		json.ShouldBe(expectedJson);

		ComplexStringObject? deserialized = JsonSerializer.Deserialize<ComplexStringObject>(json, _options);
		deserialized.ShouldNotBeNull()
			.Name.ShouldBeOfType<No<string>>()
			.ShouldBe(No<string>.Value);
	}
}
