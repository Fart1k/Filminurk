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
            var result = await _context.FavoriteLists.FirstOrDefaultAsync(x => x.FavoriteListID == id);
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
            await _context.FavoriteLists.AddAsync(newList);
            await _context.SaveChangesAsync();

            //foreach (var movieId in selectedMovies)
            //{
            //    _context.FavouriteLists.Entry
            //}

            return newList;
        }
    }
}
