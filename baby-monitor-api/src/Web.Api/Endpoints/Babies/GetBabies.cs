using Web.Api.Endpoints.Babies.Contracts;

namespace Web.Api.Endpoints.Babies;

internal sealed class GetBabies : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("babies", () =>
        {
            BabyResponse[] babies =
            [
                new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Emma", new DateOnly(2025, 6, 15)),
                new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Noah", new DateOnly(2026, 1, 3))
            ];

            return Results.Ok(babies);
        })
        .WithTags(Tags.Babies)
        .Produces<BabyResponse[]>()
        .RequireAuthorization();
    }
}
