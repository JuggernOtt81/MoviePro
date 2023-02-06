using MoviePro.Models.DataBase;
using MoviePro.Models.TMDB;
using System.Threading.Tasks;

namespace MoviePro.Services.Interfaces
{
    public interface IDataMappingService
    {
        Task<Movie> MapMovieDetailAsync(MovieDetail movie);
        ActorDetail MapActorDetail(ActorDetail actor);
        string BuildCastImage(string profilePath);
    }
}
