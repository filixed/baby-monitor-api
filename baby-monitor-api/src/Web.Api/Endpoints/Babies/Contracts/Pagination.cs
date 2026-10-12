namespace Web.Api.Endpoints.Babies.Contracts;

public sealed record Pagination(int Limit, string? NextCursor, bool HasMore);
