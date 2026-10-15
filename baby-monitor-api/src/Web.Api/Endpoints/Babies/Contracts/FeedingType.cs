using System.Text.Json.Serialization;

namespace Web.Api.Endpoints.Babies.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter<FeedingType>))]
public enum FeedingType
{
    None = 0,
    Breast = 1,
    Bottle = 2
}
