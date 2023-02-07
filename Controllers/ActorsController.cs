using Microsoft.AspNetCore.Mvc;
using MoviePro.Services;
using MoviePro.Services.Interfaces;
using System.Threading.Tasks;

namespace MoviePro.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IRemoteMovieService _tmdbMovieService;
        private readonly IDataMappingService _tmdbMappingService;


        public ActorsController(IRemoteMovieService tmdbMovieService, IDataMappingService tmdbMappingService)
        {
            _tmdbMovieService = tmdbMovieService;
            _tmdbMappingService = tmdbMappingService;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var actor = await _tmdbMovieService.ActorDetailAsync(id);
            actor = _tmdbMappingService.MapActorDetail(actor);
            return View(actor);
        }

















    }
}
