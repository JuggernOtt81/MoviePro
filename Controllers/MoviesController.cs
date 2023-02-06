using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviePro.Data;
using MoviePro.Models.Settings;
using MoviePro.Services;
using MoviePro.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MoviePro.Models.DataBase;

namespace MoviePro.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IRemoteMovieService _tmdbMovieService;
        private readonly IDataMappingService _tmdbMappingService;

        public MoviesController(ApplicationDbContext context, IOptions<AppSettings> appSettings, IImageService imageService, IRemoteMovieService tmdbMovieService, IDataMappingService tmdbMappingService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _imageService = imageService;
            _tmdbMovieService = tmdbMovieService;
            _tmdbMappingService = tmdbMappingService;
        }

        public async Task<IActionResult> Import()
        {
            var movies = await _context.Movie.Include(m => m.Collections).ToListAsync();
            return View(movies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(int id)
        {
            //1 if we have this movie, inform the user
            if (_context.Movie.Any(m => m.MovieId == id))
            {
                var localMovie = await _context.Movie.FirstOrDefaultAsync(m => m.MovieId == id);
                return RedirectToAction("Details", "Movies", new { id = localMovie.Id, local = true });
            }

            //2 get raw data from API
            var movieDetail = await _tmdbMovieService.MovieDetailAsync(id);


            //3 put data through mapping process
            var movie = await _tmdbMappingService.MapMovieDetailAsync(movieDetail);

            //4 add the new movie
            _context.Add(movie);
            await _context.SaveChangesAsync();

            //5 assign to "All" Collection
            await AddToMovieCollection(movie.Id, _appSettings.MovieProSettings.DefaultCollection.Name);

            return RedirectToAction("Import");
        }

        private async Task AddToMovieCollection(int movieId, string collectionName)
        {
            var collection = await _context.Collection.FirstOrDefaultAsync(c => c.Name == collectionName);
            _context.Add(
                new MovieCollection()
                {
                    CollectionId = collection.Id,
                    MovieId = movieId
                }
            );
            await _context.SaveChangesAsync();
        }

        private async Task AddToMovieCollection(int movieId, int collectionId)
        {
            _context.Add(
                new MovieCollection()
                {
                    CollectionId = collectionId,
                    MovieId = movieId
                }
            );
            await _context.SaveChangesAsync();
        }
    }
}
