namespace Web.Api.Endpoints.Babies.Contracts;

public sealed record BabyResponse(Guid Id, string Name, DateOnly DateOfBirth);
