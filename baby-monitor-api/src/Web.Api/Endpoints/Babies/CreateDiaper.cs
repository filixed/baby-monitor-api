using Web.Api.Endpoints.Babies.Contracts;

namespace Web.Api.Endpoints.Babies;

internal sealed class CreateDiaper : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("babies/{babyId:guid}/diapers", (Guid babyId, DiaperData request) =>
        {
            var id = Guid.NewGuid();

            return Results.Created($"/babies/{babyId}/diapers/{id}", new CreatedResponse(id));
        })
        .WithTags(Tags.Babies)
        .Produces<CreatedResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
        //.RequireAuthorization();
    }
}
