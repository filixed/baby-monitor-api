namespace Web.Api.Endpoints.Babies.Contracts;

public sealed record FeedingData(
    FeedingType FeedingType,
    int? AmountMl,
    int? DurationMinutes,
    BreastSide Side,
    DateTimeOffset Timestamp);
