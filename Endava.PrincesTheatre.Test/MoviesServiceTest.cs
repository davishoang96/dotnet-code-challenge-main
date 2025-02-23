using System.Net;
using Endava.PrincesTheatre.Models;
using Endava.PrincesTheatre.Services;
using Endava.PrincesTheatre.Services.ApiResponses;
using Moq;
using Moq.Protected;

namespace Endava.PrincesTheatre.Test;

public class MoviesServiceTest
{
    private const string BaseUrl = "https://mockapi.com.au/api/v2/";

    private const string ApiFailureResponse = "{\"message\": \"Bad Gateway\"}";

    private const string ApiCwSuccessfulResponse =
        "{\"Provider\":\"Cinema World\",\"Movies\":[{\"ID\":\"cw0076759\",\"Title\":\"Star Wars: Episode IV - A New Hope\",\"Type\":\"movie\",\"Poster\":\"http://poster-path\",\"Actors\":\"Mark Hamill, Harrison Ford, Carrie Fisher, Peter Cushing\",\"Price\":25.5}]}";

    private const string ApiFwSuccessfulResponse =
        "{\"Provider\":\"Film World\",\"Movies\":[{\"ID\":\"fw0076759\",\"Title\":\"Star Wars: Episode IV - A New Hope\",\"Type\":\"movie\",\"Poster\":\"http://poster-path\",\"Actors\":\"Mark Hamill, Harrison Ford, Carrie Fisher, Peter Cushing\",\"Price\":22.9}]}";

    [Fact]
    public async void TestSuccessfulRequestAndReturnCinemaResponses()
    {
        var mockHttpClientFactory = MockApiMovieRequests(ApiCwSuccessfulResponse, ApiFwSuccessfulResponse);
        IMoviesService moviesService = new MoviesService(mockHttpClientFactory.Object);
        var result = await moviesService.GetAllCinemaMovies();

        Assert.NotNull(result);
        Assert.IsType<List<CinemaResponse>>(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async void TestUnsuccessfulRequestsAndThrowError()
    {
        var mockHttpClientFactory = MockApiMovieRequests(ApiFailureResponse, ApiFailureResponse);
        IMoviesService moviesService = new MoviesService(mockHttpClientFactory.Object);

        await Assert.ThrowsAsync<Exception>(async () => await moviesService.GetAllCinemaMovies());
    }

    private static Mock<IHttpClientFactory> MockApiMovieRequests(string cinemaworldResponse, string filmworldResponse)
    {
        var mockHTTPMessageHandler = new Mock<HttpMessageHandler>();

        mockHTTPMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(m => m.RequestUri!.Equals(BaseUrl + "cinemaworld/movies")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(cinemaworldResponse)
            });

        mockHTTPMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(m => m.RequestUri!.Equals(BaseUrl + "filmworld/movies")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(filmworldResponse)
            });

        var httpClient = new HttpClient(mockHTTPMessageHandler.Object)
        {
            BaseAddress = new Uri(BaseUrl)
        };

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(_ => _.CreateClient("HttpClient")).Returns(httpClient);

        return mockHttpClientFactory;
    }
}