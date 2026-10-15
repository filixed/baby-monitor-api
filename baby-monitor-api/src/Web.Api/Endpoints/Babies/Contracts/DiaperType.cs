using System.Text.Json.Serialization;

namespace Web.Api.Endpoints.Babies.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter<DiaperType>))]
public enum DiaperType
{
    None = 0,
    Wet = 1,
    Dirty = 2,
    Both = 3
}
