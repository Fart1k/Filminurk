using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.FavoriteLists;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class FavoriteListsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFavoriteListsServices _favoriteListsServices;

        public FavoriteListsController
            (
                FilminurkTARpe24Context context,
                IFavoriteListsServices favouriteListsServices
            )
        {
            _context = context;
            _favoriteListsServices = favouriteListsServices;
        }

        public IActionResult Index()
        {
            var resultingLists = _context.FavoriteLists
                .OrderByDescending(y => y.ListCreatedAt)
                .Select(x => new FavoriteListsIndexViewModel
                {
                    FavoriteListID = x.FavoriteListID,
                    ListBelongsToUser = x.ListBelongsToUser,
                    IsMovieOrActor = x.IsMovieOrActor,
                    ListName = x.ListName,
                    Description = x.Description,
                    ListCreatedAt = x.ListCreatedAt,
                    Image = (List<FavoriteListsIndexImageViewModel>)_context.FileToDatabase
                        .Where(ml => ml.ListID == x.FavoriteListID)
                        .Select(li => new FavoriteListsIndexImageViewModel
                        {
                            ImageID = li.ImageID,
                            ListID = li.ListID,
                            ImageData = li.ImageData,
                            ImageTitle = li.ImageTitle,
                            Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(li.ImageData))
                        })

                });
            return View(resultingLists);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var movies = _context.Movies
                .ToList()
                .OrderBy(m => m.Title)
                .Select(mo => new MoviesIndexViewModel
                {
                    ID = mo.ID,
                    Title = mo.Title,
                    FirstPublished = mo.FirstPublished,
                    MovieGenre = mo.MovieGenre,
                });

            ViewData["allMovies"] = movies;
            ViewData["userHasSelected"] = new List<string>();
            FavoriteListUserCreateViewModel vm = new();
            return View("UserCreate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create
            (
                FavoriteListUserCreateViewModel vm,
                List<string> userHasSelected,
                List<MoviesIndexViewModel> movies
            )
        {
            List<Guid> tempParse = new();
            // tekib ajutine guid list movieid-de hoidmiseks
            foreach (var stringID in userHasSelected)
            {
                // lisame, iga stringi kohta järjendis userhasselected teisendatud guidi
                tempParse.Add(Guid.Parse(stringID));
            }

            //teeme uue DTO nimekirja jaoks
            var newListDto = new FavoriteListDTO() { };
            newListDto.ListName = vm.ListName;
            newListDto.Description = vm.Description;
            newListDto.IsMovieOrActor = vm.IsMovieOrActor;
            newListDto.IsPrivate = vm.IsPrivate;
            newListDto.ListCreatedAt = DateTime.UtcNow;
            newListDto.ListBelongsToUser = "00000000-0000-0000-0000-000000000001";
            newListDto.ListModifiedAt = DateTime.UtcNow;
            newListDto.ListDeletedAt = vm.ListDeletedAt;
            newListDto.ListOfMovies = vm.ListOfMovies;

            // lisa filmid nimekirja, olemasolevate id-de põhiselt
            var listofmovieatoadd = new List<Movie>();
            foreach (var movieId in tempParse)
            {
                Movie thismovie = (Movie)_context.Movies.Where(tm => tm.ID == movieId).ToList().Take(1);
                listofmovieatoadd.Add(thismovie);
            }
            newListDto.ListOfMovies = listofmovieatoadd;

            //List<Guid> convertedIDs = new List<Guid>();
            //if (newListDto.ListOfMovies != null)
            //{
            //    convertedIDs = MovieToId(newListDto.ListOfMovies);

            //}
            var newList = await _favoriteListsServices.Create(newListDto /*, convertedIDs*/);
            if (newList != null)
            {
                return BadRequest();
            }
            return RedirectToAction("Index", vm);
        }


        private List<Guid> MovieToId(List<Movie> listOfMovies)
        {
            var result = new List<Guid>();
            foreach (var movie in listOfMovies)
            {
                result.Add(movie.ID);
            }
            return result;
        }
    }
}
