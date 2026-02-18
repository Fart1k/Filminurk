using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto.OmdbDTOs;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.Omdb;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class OmdbController : Controller
    {
        private readonly IOmdbServices _omdbServices;
        private readonly FilminurkTARpe24Context _context;
        public OmdbController
            (
            IOmdbServices omdbServices,
            FilminurkTARpe24Context context
            )
        {
            _context = context;
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


        [HttpGet]
        public IActionResult Import(string title)
        {
            OmdbResultDTO dto = new();
            dto.Title = title;
            _omdbServices.OmdbRootSearchResult(title);
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
            vm.Description = dto.Description;
            vm.imdbRating = dto.imdbRating;
            return View("Import", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Import (OmdbResultViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var dto = new OmdbImportMovieDTO()
                {
                    Id = Guid.NewGuid(),
                    Title = vm.Title,
                    Genre = vm.Genre,
                    Director = vm.Director,
                    Actors = vm.Actors?.Split(',').Select(a => a.Trim()).ToList(),
                    Description = vm.Description,
                    CurrentRating = double.TryParse(vm.imdbRating, out double rating) ? rating : (double?)null,
                    EntryCreatedAt = DateTime.Now,
                    EntryModifiedAt = DateTime.Now
                };

                if (vm.Released == "N/A")
                {
                    dto.FirstPublished = DateOnly.MinValue;
                }
                else
                {
                    dto.FirstPublished = dto.FirstPublished = DateOnly.Parse(vm.Released);
                }

                    var createdMovie = await _omdbServices.CreateMovieFromOmdb(dto);

                if (createdMovie == null)
                {
                    return NotFound();
                }
            }
            return View(nameof(Index));
        }
    }
}
