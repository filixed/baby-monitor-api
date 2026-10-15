using Web.Api.Endpoints.Babies.Contracts;

namespace Web.Api.Endpoints.Babies;

internal sealed class CreateWeight : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("babies/{babyId:guid}/weights", (Guid babyId, WeightData request) =>
        {
            var id = Guid.NewGuid();

            return Results.Created($"/babies/{babyId}/weights/{id}", new CreatedResponse(id));
        })
        .WithTags(Tags.Babies)
        .Produces<CreatedResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
        //.RequireAuthorization();
    }
}
