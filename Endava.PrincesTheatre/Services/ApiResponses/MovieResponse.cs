namespace Endava.PrincesTheatre.Services.ApiResponses
{
    public record MovieResponse(string Id, string Title, string Type, string Poster, string Actors, decimal Price);
}