using Web.Api.Endpoints.Babies.Contracts;

namespace Web.Api.Endpoints.Babies;

internal sealed class CreateSleep : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("babies/{babyId:guid}/sleeps", (Guid babyId, SleepData request) =>
        {
            var id = Guid.NewGuid();

            return Results.Created($"/babies/{babyId}/sleeps/{id}", new CreatedResponse(id));
        })
        .WithTags(Tags.Babies)
        .Produces<CreatedResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
