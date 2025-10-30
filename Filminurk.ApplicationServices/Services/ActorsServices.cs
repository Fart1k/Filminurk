using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;

namespace Filminurk.ApplicationServices.Services
{
    public class ActorsServices : IActorServices
    {
        private readonly FilminurkTARpe24Context _context;
        public ActorsServices
            (
                FilminurkTARpe24Context context
            )
        {
            _context = context;
        }

        public async Task<Actors> Create(ActorsDTO dto)
        {
            Actors actors = new Actors();
            actors.ActorID = Guid.NewGuid();
            actors.FirstName = dto.FirstName;
            actors.LastName = dto.LastName;
            actors.NickName = dto.NickName;
            actors.MoviesActedFor = dto.MoviesActedFor;
            actors.Age = dto.Age;
            actors.Gender = dto.Gender;
            actors.ActorRegion = dto.ActorRegion;
            actors.EntryCreatedAt = DateTime.Now;
            actors.EntryModifiedAt = DateTime.Now;

            await _context.Actors.AddAsync(actors);
            await _context.SaveChangesAsync();

            return actors;
        }



    }
}
