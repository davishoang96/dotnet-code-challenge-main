using Endava.PrincesTheatre.Controllers;
using Endava.PrincesTheatre.Models;
using Endava.PrincesTheatre.Services;
using Endava.PrincesTheatre.Services.ApiResponses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Endava.PrincesTheatre.Test;

public class MoviesControllerTest
{
    [Fact]
    public async void TestMoviesControllerIndexReturnsCorrectlyMappedView()
    {
        var mockMoviesService = new Mock<IMoviesService>();
        mockMoviesService.Setup(service => service.GetAllCinemaMovies())
            .ReturnsAsync(GetTestCinemaResponses());

        var controller = new MoviesController(mockMoviesService.Object);

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<MoviesViewModel>(
            viewResult.ViewData.Model);
        var movie = Assert.Single(model.Movies);
        Assert.IsType<MovieModel>(movie);
        Assert.Equal("Title1", movie.Title);
        Assert.Equal("Action", movie.Type);
        Assert.Equal("List of Actors", movie.Actors);
        Assert.Equal("http://poster-path", movie.Poster);
        Assert.Equal("low-price", movie.Prices!.First().Provider);
        Assert.Equal("high-price", movie.Prices!.Last().Provider);
        Assert.Equal(15, movie.Prices!.First().Price);
        Assert.Equal(30, movie.Prices!.Last().Price);
    }

    private static List<CinemaResponse> GetTestCinemaResponses()
    {
        CinemaResponse cinemaHighPrice = new CinemaResponse
        (
            Provider: "high-price",
            Movies: new List<MovieResponse>()
            {
                new
                (
                    Id: "m1",
                    Title: "Title1",
                    Type: "Action",
                    Actors: "List of Actors",
                    Poster: "http://poster-path",
                    Price: 30
                )
            }
        );

        CinemaResponse cinemaLowPrice = new CinemaResponse
        (
            Provider: "low-price",
            Movies: new List<MovieResponse>
            {
                new
                (
                    Id: "m1",
                    Title: "Title1",
                    Type: "Action",
                    Actors: "List of Actors",
                    Poster: "http://poster-path",
                    Price: 15
                )
            }
        );

        return new List<CinemaResponse> { cinemaHighPrice, cinemaLowPrice };
    }
}