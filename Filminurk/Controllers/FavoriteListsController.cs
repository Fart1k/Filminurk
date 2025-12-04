using System.Threading.Tasks;
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
                    ListDeletedAt = (DateTime)x.ListDeletedAt,
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
        public IActionResult Create()
        {
            var movies = _context.Movies
                .OrderBy(m => m.Title)
                .Select(mo => new MoviesIndexViewModel
                {
                    ID = mo.ID,
                    Title = mo.Title,
                    FirstPublished = mo.FirstPublished,
                    MovieGenre = mo.MovieGenre,
                }).ToList();

            ViewData["allMovies"] = movies;
            ViewData["userHasSelected"] = new List<string>();
            FavoriteListUserCreateViewModel vm = new();
            return View("UserCreate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> UserCreate
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
            newListDto.ListBelongsToUser = Guid.NewGuid().ToString();
            newListDto.ListModifiedAt = DateTime.UtcNow;
            newListDto.ListDeletedAt = vm.ListDeletedAt;

            // lisa filmid nimekirja, olemasolevate id-de põhiselt
            var listofmovieatoadd = new List<Movie>();
            foreach (var movieId in tempParse)
            {
                var thismovie = _context.Movies.Where(tm => tm.ID == movieId).ToArray().First();
                listofmovieatoadd.Add((Movie)thismovie);
            }
            newListDto.ListOfMovies = listofmovieatoadd;

            //List<Guid> convertedIDs = new List<Guid>();
            //if (newListDto.ListOfMovies != null)
            //{
            //    convertedIDs = MovieToId(newListDto.ListOfMovies);

            //}
            var newList = await _favoriteListsServices.Create(newListDto /*, convertedIDs*/);
            if (newList == null)
            {
                return BadRequest();
            }
            return RedirectToAction("Index", vm);
        }

        //

        [HttpGet]
        public async Task<IActionResult> UserDetails(Guid id, Guid thisuserid)
        {
            

            if (id == null || thisuserid == null)
            {
                return BadRequest();
            }
            var thisList = _context.FavoriteLists
                .Where(l => l.FavoriteListID == id && l.ListBelongsToUser == thisuserid.ToString())
                .Select(
                stl => new FavoriteListUserDetailsViewModel
                {
                    FavoriteListID = stl.FavoriteListID,
                    ListBelongsToUser = stl.ListBelongsToUser,
                    IsMovieOrActor = stl.IsMovieOrActor,
                    ListName = stl.ListName,
                    Description = stl.Description,
                    IsPrivate = stl.IsPrivate,
                    ListOfMovies = stl.ListOfMovies,
                    IsReported = stl.IsReported,
                    //Image = _context.FilesToDatabase
                    //    .Where(i => i.ListID == stl.FavouriteListID)
                    //    .Select(si => new FavouriteListIndexImageViewModel
                    //    {
                    //        ImageID = si.ImageID,
                    //        ListID = si.ListID,
                    //        ImageData = si.ImageData,
                    //        ImageTitle = si.ImageTitle,
                    //        Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(si.ImageData))
                    //    }).ToList()
                }).First();

            if (thisList == null)
            {
                return NotFound();
            }

            return View("Details", thisList);
        }

        //
        //[HttpGet]

        //public async Task<IActionResult> UserTogglePrivacy(Guid id, Guid thisuserid)
        //{
        //    if (id == null || thisuserid == null)
        //    {
        //        return BadRequest();
        //    }
        //    var thisList = _context.FavoriteLists
        //        .Where(tl => tl.FavoriteListID == id && tl.ListBelongsToUser == thisuserid.ToString())
        //        .Select
        //        (
        //        stl => new FavoriteListUserDetailsViewModel
        //        {
        //            FavoriteListID = stl.FavoriteListID,
        //            ListBelongsToUser = stl.ListBelongsToUser,
        //            IsMovieOrActor = stl.IsMovieOrActor,
        //            ListName = stl.ListName,
        //            Description = stl.Description,
        //            IsPrivate = stl.IsPrivate,
        //            ListOfMovies = stl.ListOfMovies,
        //            IsReported = stl.IsReported,
        //            //Image = _context.FilesToDatabase
        //            //    .Where(i => i.ListID == stl.FavouriteListID)
        //            //    .Select(si => new FavouriteListIndexImageViewModel
        //            //    {
        //            //        ImageID = si.ImageID,
        //            //        ListID = si.ListID,
        //            //        ImageData = si.ImageData,
        //            //        ImageTitle = si.ImageTitle,
        //            //        Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(si.ImageData))
        //            //    }).ToList()
        //        }).First();
        //    //add vd atr here later, for checking if user&admin

        //    if (thisList == null)
        //    {
        //        return NotFound();
        //    }

        //    return View("UserTogglePrivacy", thisList);
        //}

        [HttpPost]
        public async Task<IActionResult> UserTogglePrivacy(Guid id)
        {
            FavoriteList thisList = await _favoriteListsServices.DetailsAsync(id);

            FavoriteListDTO updatedList = new FavoriteListDTO();
            updatedList.FavoriteListID = thisList.FavoriteListID;
            updatedList.ListBelongsToUser = thisList.ListBelongsToUser;
            updatedList.ListName = thisList.ListName;
            updatedList.Description = thisList.Description;
            updatedList.IsPrivate = thisList.IsPrivate;
            updatedList.ListOfMovies = thisList.ListOfMovies;
            updatedList.IsReported = thisList.IsReported;
            updatedList.IsMovieOrActor = thisList.IsMovieOrActor;
            updatedList.ListCreatedAt = thisList.ListCreatedAt;
            updatedList.ListModifiedAt = DateTime.Now;
            updatedList.ListDeletedAt = thisList.ListDeletedAt;

            ViewData["UpdateServiceType"] = "Private";

            var result = await _favoriteListsServices.Update(updatedList, "Private");

            if (result == null)
            {
                return NotFound();
            }
            //if (result.IsPrivate != !result.IsPrivate)
            //{
            //    return BadRequest();
            //}

            //return RedirectToAction("UserDetails", result.FavoriteListID);
            return RedirectToAction(nameof(Index));

        }

        // Delete
        [HttpPost]
        public async Task<IActionResult> UserDelete(Guid id)
        {

            var deletedList = await _favoriteListsServices.DetailsAsync(id);
            deletedList.ListDeletedAt = DateTime.Now;

            var dto = new FavoriteListDTO();
            dto.FavoriteListID = deletedList.FavoriteListID;
            dto.ListBelongsToUser = deletedList.ListBelongsToUser;
            dto.ListName = deletedList.ListName;
            dto.Description = deletedList.Description;
            dto.IsPrivate = deletedList.IsPrivate;
            dto.ListOfMovies = deletedList.ListOfMovies;
            dto.IsReported = deletedList.IsReported;
            dto.IsMovieOrActor = deletedList.IsMovieOrActor;
            dto.ListCreatedAt = deletedList.ListCreatedAt;
            dto.ListModifiedAt = DateTime.Now;
            dto.ListDeletedAt = DateTime.Now;

            ViewData["UpdateServiceType"] = "Delete";

            var result = await _favoriteListsServices.Update(dto, "Delete");
            if (result == null)
            {
                return NotFound();
            }
            

            return RedirectToAction(nameof(Index));

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
