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
        public async Task<IActionResult> SearchMovie(string movieTitle)
        {
            var dto = await _omdbServices.OmdbRootSearchResult(movieTitle);

            if (dto == null || dto.Response == "False")
            {
                return View("Import", new OmdbResultViewModel
                {
                    Title = movieTitle,
                    Genre = Genre.Unknown
                });
            }

            OmdbResultViewModel vm = new();

            vm.Title = dto.Title;
            vm.Released = dto.Released;

            if (!string.IsNullOrEmpty(dto.Genre) &&
                Enum.TryParse(dto.Genre.Split(',')[0].Trim(), true, out Genre parsedGenre))
            {
                vm.Genre = parsedGenre;
            }
            else
            {
                vm.Genre = Genre.Unknown;
            }

            vm.Director = dto.Director;
            vm.Actors = dto.Actors;
            vm.Description = dto.Plot;
            vm.imdbRating = dto.imdbRating;

            return View("Import", vm);
        }

    }
}
