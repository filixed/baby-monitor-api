using System.Text.Json.Serialization;

namespace Web.Api.Endpoints.Babies.Contracts;

[JsonDerivedType(typeof(FeedingEvent))]
[JsonDerivedType(typeof(DiaperEvent))]
[JsonDerivedType(typeof(SleepEvent))]
[JsonDerivedType(typeof(WeightEvent))]
public abstract record TimelineEvent(Guid Id, string Type, DateTimeOffset Timestamp);

public sealed record FeedingEvent(Guid Id, DateTimeOffset Timestamp, FeedingData Data)
    : TimelineEvent(Id, "feeding", Timestamp);

public sealed record DiaperEvent(Guid Id, DateTimeOffset Timestamp, DiaperData Data)
    : TimelineEvent(Id, "diaper", Timestamp);

public sealed record SleepEvent(Guid Id, DateTimeOffset Timestamp, SleepData Data)
    : TimelineEvent(Id, "sleep", Timestamp);

public sealed record WeightEvent(Guid Id, DateTimeOffset Timestamp, WeightData Data)
    : TimelineEvent(Id, "weight", Timestamp);
