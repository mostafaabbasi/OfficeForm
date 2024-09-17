using System.Text.Json.Serialization;

namespace OfficeFormExample.Models;

public class SubmitFormOfficeModel
{
    [JsonPropertyName("startDate")]
    public DateTime startDate { get; set; }
    
    [JsonPropertyName("submitDate")]
    public DateTime submitDate { get; set; }
    
    [JsonPropertyName("answers")]
    public string answers { get; set; }
}

// public class RawJsonConverter : JsonConverter<string>
// {
//     public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         using JsonDocument doc = JsonDocument.ParseValue(ref reader);
//         return doc.RootElement.GetRawText();
//     }
//
//     public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
//     {
//         using JsonDocument doc = JsonDocument.Parse(value);
//         doc.RootElement.WriteTo(writer);
//     }
// }