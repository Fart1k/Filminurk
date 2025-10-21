using Filminurk.Models.Movies;
using Filminurk.Core.Dto;
using Filminurk.Data;
using Microsoft.AspNetCore.Mvc;
using Filminurk.Core.ServiceInterface;

namespace Filminurk.Controllers
{
    public class MoviesController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IMovieServices _movieService;
        public MoviesController(FilminurkTARpe24Context context,
            IMovieServices movieServices)
        {
            _context = context;
            _movieService = movieServices;
        }

        public IActionResult Index()
        {
            var result = _context.Movies.Select(x => new MoviesIndexViewModel
            {
                ID = x.ID,
                Title = x.Title,
                FirstPublished = x.FirstPublished,
                CurrentRating = x.CurrentRating,
                Warnings = x.Warnings,

            });
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            MoviesCreateViewModel result = new();
            return View("Create", result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoviesCreateViewModel vm)
        {
            var dto = new MoviesDTO()
            {
                ID = vm.ID,
                Title = vm.Title,
                FirstPublished = vm.FirstPublished,
                Genre = vm.Genre,
                CurrentRating = vm.CurrentRating,
                Actors = vm.Actors,
                EntryCreatedAt = vm.EntryCreatedAt,
                EntryModifiedAt = vm.EntryModifiedAt,
                Director = vm.Director,
                Description = vm.Description,
            };
            var result = await _movieService.Create(dto);
            if (result == null)
            {
                RedirectToAction(nameof(Index));
            }
            RedirectToAction(nameof(Index));
        }
    }
}