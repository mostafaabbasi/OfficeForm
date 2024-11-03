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