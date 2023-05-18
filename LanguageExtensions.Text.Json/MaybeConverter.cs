using System.Text.Json;
using System.Text.Json.Serialization;

namespace LanguageExtensions.Text.Json;

public class MaybeConverterFactory : JsonConverterFactory
{
	public override bool CanConvert(Type typeToConvert)
		=> typeToConvert.IsGenericType
		&& typeToConvert.GetGenericTypeDefinition() == typeof(Maybe<>);

	public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		Type converterType = typeof(MaybeConverter<>).MakeGenericType(typeToConvert.GetGenericArguments()[0]);
		return Activator.CreateInstance(converterType) as JsonConverter;
	}
}

public class MaybeConverter<Data> : JsonConverter<Maybe<Data>>
	where Data : notnull
{
	public override bool HandleNull => true;

	public override Maybe<Data> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		object? result = JsonSerializer.Deserialize(ref reader, typeof(Data?));

		return result is not null
			? (Data)result
			: No<Data>.Value;
	}

	public override void Write(Utf8JsonWriter writer, Maybe<Data> value, JsonSerializerOptions options)
	{
		if (value is Some<Data> someData)
		{
			JsonSerializer.Serialize(writer, someData.Value, someData.Value.GetType());
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}
