using System.Text.Json.Serialization;

namespace Web.Api.Endpoints.Babies.Contracts;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(FeedingEvent), "feeding")]
[JsonDerivedType(typeof(DiaperEvent),  "diaper")]
[JsonDerivedType(typeof(SleepEvent),   "sleep")]
[JsonDerivedType(typeof(WeightEvent),  "weight")]
public abstract record TimelineEvent(Guid Id, DateTimeOffset Timestamp);

public sealed record FeedingEvent(Guid Id, DateTimeOffset Timestamp, FeedingData Data)
    : TimelineEvent(Id, Timestamp);

public sealed record DiaperEvent(Guid Id, DateTimeOffset Timestamp, DiaperData Data)
    : TimelineEvent(Id, Timestamp);

public sealed record SleepEvent(Guid Id, DateTimeOffset Timestamp, SleepData Data)
    : TimelineEvent(Id, Timestamp);

public sealed record WeightEvent(Guid Id, DateTimeOffset Timestamp, WeightData Data)
    : TimelineEvent(Id, Timestamp);
