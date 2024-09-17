using System.Text.Json.Serialization;

namespace OfficeFormExample.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RequestTypeEnum
{
    Refund,
    ReIssue
}