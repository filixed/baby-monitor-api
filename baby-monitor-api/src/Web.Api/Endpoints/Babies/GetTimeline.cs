using Microsoft.AspNetCore.Mvc;
using Web.Api.Endpoints.Babies.Contracts;

namespace Web.Api.Endpoints.Babies;

internal sealed class GetTimeline : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("babies/{babyId:guid}/timeline", (
            Guid babyId,
            [FromQuery] DateOnly date,
            [FromQuery] int? limit,
            [FromQuery] string? cursor) =>
        {
            var at = new DateTimeOffset(date, new TimeOnly(8, 0), TimeSpan.Zero);

            TimelineEvent[] items =
            [
                new FeedingEvent(
                    Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    at,
                    new FeedingData(FeedingType.Breast, null, 15, BreastSide.Left, at)),
                new DiaperEvent(
                    Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    at.AddHours(1),
                    new DiaperData(DiaperType.Wet, "Routine change", at.AddHours(1))),
                new SleepEvent(
                    Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    at.AddHours(2),
                    new SleepData("Morning nap", at.AddHours(2), at.AddHours(3.5))),
                new WeightEvent(
                    Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    at.AddHours(4),
                    new WeightData(4.8, at.AddHours(4)))
            ];

            return Results.Ok(new TimelinePage(date, items, new Pagination(limit ?? 20, null, false)));
        })
        .WithTags(Tags.Babies)
        .Produces<TimelinePage>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
        //.RequireAuthorization();
    }
}
