using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Endava.PrincesTheatre.Models;
using Endava.PrincesTheatre.Models.Mappers;
using Endava.PrincesTheatre.Services;

namespace Endava.PrincesTheatre.Controllers;

public class MoviesController : Controller
{
    private readonly IMoviesService _moviesService;

    public MoviesController(IMoviesService moviesService)
    {
        this._moviesService = moviesService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var cinemaMovies = await _moviesService.GetAllCinemaMovies();
            var movies = cinemaMovies.MapToMovies();
            
            return View(new MoviesViewModel (Movies: movies));
        }
        catch
        {
            return RedirectToAction("Error");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}