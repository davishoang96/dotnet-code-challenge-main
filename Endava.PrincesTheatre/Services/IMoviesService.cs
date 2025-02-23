using Endava.PrincesTheatre.Models;
using Endava.PrincesTheatre.Services.ApiResponses;

namespace Endava.PrincesTheatre.Services
{
    public interface IMoviesService
    {
        Task<List<CinemaResponse>> GetAllCinemaMovies();
    }
}

