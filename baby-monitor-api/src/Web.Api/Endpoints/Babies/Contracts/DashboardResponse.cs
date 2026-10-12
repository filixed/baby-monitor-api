namespace Web.Api.Endpoints.Babies.Contracts;

public sealed record DashboardResponse(int Feedings, int Diapers, int Sleep, double Weight);
