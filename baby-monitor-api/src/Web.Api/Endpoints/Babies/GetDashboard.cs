using Web.Api.Endpoints.Babies.Contracts;

namespace Web.Api.Endpoints.Babies;

internal sealed class GetDashboard : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("babies/{babyId:guid}/dashboard", (Guid babyId) =>
            Results.Ok(new DashboardResponse(Feedings: 6, Diapers: 5, Sleep: 840, Weight: 4.8)))
        .WithTags(Tags.Babies)
        .Produces<DashboardResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound);
        //.RequireAuthorization();
    }
}
