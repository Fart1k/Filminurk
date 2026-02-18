using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto.OmdbDTOs;
using Filminurk.Core.ServiceInterface;
using Filminurk.Models.Omdb;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class OmdbController : Controller
    {
        private readonly IOmdbServices _omdbServices;
        public OmdbController(IOmdbServices omdbServices)
        {
            _omdbServices = omdbServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FindMovie(OmdbIndexViewModel vm)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("SearchMovie", "Omdb", new { movieTitle = vm.Title });
            }
            return View(nameof(Index));
        }

        [HttpGet]
        public IActionResult SearchMovie(string movieTitle)
        {
            OmdbResultDTO dto = new();
            dto.Title = movieTitle;
            _omdbServices.OmdbRootSearchResult(dto);
            OmdbResultViewModel vm = new();
            vm.Title = dto.Title;
            vm.Released = dto.Released;
            if (!string.IsNullOrEmpty(dto.Genre) &&
                Enum.IsDefined(typeof(Genre), dto.Genre))
            {
                vm.Genre = (Genre)Enum.Parse(typeof(Genre), dto.Genre);
            }
            else
            {
                vm.Genre = Genre.Unknown;
            }

            vm.Director = dto.Director;
            vm.Actors = dto.Actors;
            vm.Description = dto.Description;
            vm.imdbRating = dto.imdbRating;

            return View("Import", vm);
        }
    }
}
