using System.Text.Json.Serialization;

namespace Web.Api.Endpoints.Babies.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter<BreastSide>))]
public enum BreastSide
{
    None = 0,
    Left = 1,
    Right = 2
}
