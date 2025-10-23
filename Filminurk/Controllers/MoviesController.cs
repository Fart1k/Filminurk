using Filminurk.Data;
using Filminurk.Models.Movies;
using Filminurk.Core.Dto;
using Microsoft.AspNetCore.Mvc;
using Filminurk.Core.ServiceInterface;
using Microsoft.EntityFrameworkCore;

namespace Filminurk.Controllers
{
    public class MoviesController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IMovieServices _movieServices;
        private readonly IFilesServices _filesServices; //piltide jaoks
        public MoviesController
            (
                FilminurkTARpe24Context context, 
                IMovieServices movieServices, 
                IFilesServices filesServices //piltide jaoks
            )
        {
            _context = context;
            _movieServices = movieServices;
            _filesServices = filesServices; //piltide jaoks
        }
        public IActionResult Index()
        {
            var result = _context.Movies.Select(x => new MoviesIndexViewModel
            {
                ID = x.ID,
                Title = x.Title,
                FirstPublished = x.FirstPublished,
                CurrentRating = x.CurrentRating,
                MovieGenre = x.MovieGenre,

            });
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            MoviesCreateUpdateViewModel result = new();
            return View("CreateUpdate", result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoviesCreateUpdateViewModel vm)
        {
            var dto = new MoviesDTO()
            {
                ID = vm.ID,
                Title = vm.Title,
                FirstPublished = vm.FirstPublished,
                MovieGenre = vm.MovieGenre,
                CurrentRating = vm.CurrentRating,
                Actors = vm.Actors,
                EntryCreatedAt = vm.EntryCreatedAt,
                EntryModifiedAt = vm.EntryModifiedAt,
                Director = vm.Director,
                Description = vm.Description,
                Files = vm.Files,
                FileToApiDTOs = vm.Images
                .Select(x => new FileToApiDTO
                {
                    ImageID = x.ImageID,
                    FilePath = x.FilePath,
                    MovieID = x.MovieID,
                    IsPoster = x.IsPoster,
                }).ToArray()
            };
            var result = await _movieServices.Create(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var movie = await _movieServices.DetailsAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            var vm = new MoviesDetailsViewModel();
            vm.ID = movie.ID;
            vm.Title = movie.Title;
            vm.FirstPublished = movie.FirstPublished;
            vm.MovieGenre = movie.MovieGenre;
            vm.CurrentRating = movie.CurrentRating;
            vm.Actors = movie.Actors;
            vm.EntryCreatedAt = movie.EntryCreatedAt;
            vm.EntryModifiedAt = movie.EntryModifiedAt;
            vm.Director = movie.Director;
            vm.Description = movie.Description;
            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var movie = await _movieServices.DetailsAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var images = await _context.FilesToApi
                .Where(x => x.MovieID == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageID = id
                }).ToArrayAsync();

            var vm = new MoviesCreateUpdateViewModel();

            vm.ID = movie.ID;
            vm.Title = movie.Title;
            vm.Description = movie.Description;
            vm.FirstPublished = movie.FirstPublished;
            vm.MovieGenre = movie.MovieGenre;
            vm.CurrentRating = movie.CurrentRating;
            vm.Actors = movie.Actors;
            vm.EntryCreatedAt = movie.EntryCreatedAt;
            vm.EntryModifiedAt = movie.EntryModifiedAt;
            vm.Director = movie.Director;
            vm.Images.AddRange(images);

            return View("CreateUpdate", vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(MoviesCreateUpdateViewModel vm)
        {
            var dto = new MoviesDTO()
            {
                ID = vm.ID,
                Title = vm.Title,
                Description = vm.Description,
                FirstPublished = vm.FirstPublished,
                MovieGenre = vm.MovieGenre,
                CurrentRating = vm.CurrentRating,
                Actors = vm.Actors,
                EntryCreatedAt = vm.EntryCreatedAt,
                EntryModifiedAt = vm.EntryModifiedAt,
                Director = vm.Director,
                Files = vm.Files,
                FileToApiDTOs = vm.Images
                .Select(x => new FileToApiDTO
                {
                    ImageID = x.ImageID,
                    MovieID = x.MovieID,
                    FilePath = x.FilePath,
                }).ToArray()
            };
            var result = await _movieServices.Update(dto);
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid ID)
        {
            var movie = await _movieServices.DetailsAsync(ID);
            if (movie == null)
            {
                return NotFound();
            }

            var images = await _context.FilesToApi
                .Where(x => x.MovieID == ID)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageID = y.ImageID,
                }).ToArrayAsync();

            var vm = new MoviesDeleteViewModel();

            vm.ID = movie.ID;
            vm.Title = movie.Title;
            vm.FirstPublished = movie.FirstPublished;
            vm.MovieGenre = movie.MovieGenre;
            vm.CurrentRating = movie.CurrentRating;
            vm.Actors = movie.Actors;
            vm.EntryCreatedAt = movie.EntryCreatedAt;
            vm.EntryModifiedAt = movie.EntryModifiedAt;
            vm.Director = movie.Director;
            vm.Description = movie.Description;
            vm.Images.AddRange(images);

            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid ID)
        {
            var movie = await _movieServices.Delete(ID);
            if (movie == null) { return NotFound(); }
            return RedirectToAction(nameof(Index));
        }
    }
}