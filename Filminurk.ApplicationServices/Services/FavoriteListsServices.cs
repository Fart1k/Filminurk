using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.ApplicationServices.Services
{
    public class FavoriteListsServices : IFavoriteListsServices
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFilesServices _filesServices;

        public FavoriteListsServices
            (
                FilminurkTARpe24Context context,
                IFilesServices filesServices
            )
        {
            _context = context;
            _filesServices = filesServices;
        }

        public async Task<FavoriteList> DetailsAsync(Guid id)
        {
            var result = await _context.FavoriteLists
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FavoriteListID == id);
            return result;
        }

        public async Task<FavoriteList> Create(FavoriteListDTO dto /*, List<Movie> selectedMovies */)
        {
            FavoriteList newList = new();
            newList.FavoriteListID = Guid.NewGuid();
            newList.ListName = dto.ListName;
            newList.Description = dto.Description;
            newList.ListCreatedAt = dto.ListCreatedAt;
            newList.ListModifiedAt = dto.ListModifiedAt;
            newList.ListDeletedAt = dto.ListDeletedAt;
            newList.ListOfMovies = dto.ListOfMovies;
            newList.ListBelongsToUser = dto.ListBelongsToUser;
            await _context.FavoriteLists.AddAsync(newList);
            await _context.SaveChangesAsync();

            //foreach (var movieId in selectedMovies)
            //{
            //    _context.FavouriteLists.Entry
            //}

            return newList;
        }

        // Update
        public async Task<FavoriteList> Update(FavoriteListDTO privatedList, string typeOfMethod)
        {
            FavoriteList privatedListInDB = new();

            privatedListInDB.FavoriteListID = privatedList.FavoriteListID;
            privatedListInDB.ListBelongsToUser = privatedList.ListBelongsToUser;
            privatedListInDB.IsMovieOrActor = privatedList.IsMovieOrActor;
            privatedListInDB.ListName = privatedList.ListName;
            privatedListInDB.Description = privatedList.Description;
            privatedListInDB.IsPrivate = privatedList.IsPrivate;
            privatedListInDB.ListOfMovies = privatedList.ListOfMovies;
            privatedListInDB.ListCreatedAt = privatedList.ListCreatedAt;
            privatedListInDB.ListDeletedAt= privatedList.ListDeletedAt;
            privatedListInDB.ListModifiedAt= privatedList.ListModifiedAt;

            if (typeOfMethod == "Delete")
            {
                _context.FavoriteLists.Attach(privatedListInDB);
                _context.Entry(privatedListInDB).Property(l => l.ListDeletedAt).IsModified = true;
            }

            else if (typeOfMethod == "Private")
            {
                _context.FavoriteLists.Attach(privatedListInDB);
                _context.Entry(privatedListInDB).Property(l => l.IsPrivate).IsModified = true;
            }
            _context.Entry(privatedListInDB).Property(l => l.ListModifiedAt).IsModified = true;
            await _context.SaveChangesAsync();
            return privatedListInDB;


        }
    }
}
