using Web.Api.Endpoints.Babies.Contracts;

namespace Web.Api.Endpoints.Babies;

internal sealed class CreateFeeding : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("babies/{babyId:guid}/feedings", (Guid babyId, FeedingData request) =>
        {
            var id = Guid.NewGuid();

            return Results.Created($"/babies/{babyId}/feedings/{id}", new CreatedResponse(id));
        })
        .WithTags(Tags.Babies)
        .Produces<CreatedResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
