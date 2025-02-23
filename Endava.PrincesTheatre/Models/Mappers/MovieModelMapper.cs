using Endava.PrincesTheatre.Services.ApiResponses;

namespace Endava.PrincesTheatre.Models.Mappers;

public static class MovieModelMapper
{
    public static List<MovieModel> MapToMovies(this IEnumerable<CinemaResponse> cinemaResponses)
    {
        Dictionary<string, MovieModel> map = new Dictionary<string, MovieModel>();

        foreach (CinemaResponse c in cinemaResponses)
        {
            foreach (MovieResponse m in c.Movies)
            {
                PriceModel price = new PriceModel(Provider: c.Provider, Price: m.Price);
                MovieModel movie = map.GetValueOrDefault(m.Title,
                    new MovieModel(
                        Title: m.Title,
                        Type: m.Type,
                        Poster: m.Poster,
                        Actors: m.Actors,
                        Prices: new List<PriceModel>()
                    ));
                movie.Prices!.Add(price);
                movie.Prices!.Sort((a, b) => a.Price.CompareTo(b.Price));
                map[m.Title] = movie;
            }
        }

        return map.Values.ToList();
    }
}