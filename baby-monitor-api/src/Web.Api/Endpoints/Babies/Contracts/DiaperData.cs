namespace Web.Api.Endpoints.Babies.Contracts;

public sealed record DiaperData(DiaperType DiaperType, string? Description, DateTimeOffset Timestamp);
