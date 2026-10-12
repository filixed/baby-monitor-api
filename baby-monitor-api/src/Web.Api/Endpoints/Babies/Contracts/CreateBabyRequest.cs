namespace Web.Api.Endpoints.Babies.Contracts;

public sealed record CreateBabyRequest(string UserId, string Name, DateOnly DateOfBirth);
