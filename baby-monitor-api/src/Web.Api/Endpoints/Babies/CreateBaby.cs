using Web.Api.Endpoints.Babies.Contracts;

namespace Web.Api.Endpoints.Babies;

internal sealed class CreateBaby : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("babies", (CreateBabyRequest request) =>
        {
            var id = Guid.NewGuid();

            return Results.Created($"/babies/{id}", new CreatedResponse(id));
        })
        .WithTags(Tags.Babies)
        .Produces<CreatedResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequireAuthorization();
    }
}
