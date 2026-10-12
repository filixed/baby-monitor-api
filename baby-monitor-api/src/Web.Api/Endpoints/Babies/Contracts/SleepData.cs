namespace Web.Api.Endpoints.Babies.Contracts;

public sealed record SleepData(string? Description, DateTimeOffset StartTimestamp, DateTimeOffset EndTimestamp);
