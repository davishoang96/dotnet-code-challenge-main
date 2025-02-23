using Endava.PrincesTheatre.Services.ApiResponses;

namespace Endava.PrincesTheatre.Services
{
    public class MoviesService: IMoviesService
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public MoviesService(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }

        public async Task<List<CinemaResponse>> GetAllCinemaMovies()
        {
            var cinemaWorldMovies = await GetCinemaMovies("cinemaworld");
            var filmWorldMovies = await GetCinemaMovies("filmworld");

            return new List<CinemaResponse> { cinemaWorldMovies, filmWorldMovies };
        }

        private async Task<CinemaResponse> GetCinemaMovies(string provider)
        {
            var httpClient = _clientFactory.CreateClient("HttpClient");
            var response = await httpClient.GetFromJsonAsync<CinemaResponse>($"{provider}/movies");
            
            if (response == null || response.Provider == null && response.Movies == null)
                throw new Exception("An error has occurred while fetching movie data");
            
            return response;
        }
    }
}
