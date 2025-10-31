using AspNetCoreGeneratedDocument;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.Actors;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Filminurk.Controllers
{
    public class ActorsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IActorServices _actorServices;


        public ActorsController
            (
                FilminurkTARpe24Context context,
                IActorServices actorServices
            )
        {
            _context = context;
            _actorServices = actorServices;
        }

        public IActionResult Index()
        {
            var result = _context.Actors.Select(x => new ActorsIndexViewModel
            {
                ActorID = x.ActorID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Age = x.Age,
                Gender = x.Gender,
            });
            return View(result);
        }

        // Create
        [HttpGet]
        public IActionResult Create()
        {
            ActorsCreateUpdateViewModel result = new();
            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActorsCreateUpdateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var dto = new ActorsDTO()
                {
                    ActorID = vm.ActorID,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    NickName = vm.NickName,
                    MoviesActedFor = vm.MoviesActedFor,
                    PortraitID = vm.PortraitID,
                    Age = vm.Age,
                    Gender = vm.Gender,
                    ActorRegion = vm.ActorRegion,
                    EntryCreatedAt = vm.EntryCreatedAt,
                    EntryModifiedAt = vm.EntryModifiedAt,
                };
                var result = await _actorServices.Create(dto);
                if (result == null)
                {
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // Details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var actor = await _actorServices.DetailsAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            var vm = new ActorsDetailsViewModel();
            vm.ActorID = actor.ActorID;
            vm.FirstName = actor.FirstName;
            vm.LastName = actor.LastName;
            vm.NickName = actor.NickName;
            vm.MoviesActedFor = actor.MoviesActedFor;
            vm.Age = actor.Age;
            vm.Gender = actor.Gender;
            vm.ActorRegion = actor.ActorRegion;
            vm.EntryCreatedAt = actor.EntryCreatedAt;
            vm.EntryModifiedAt = actor.EntryModifiedAt;
            return View(vm);
        }

        // Update
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var actor = await _actorServices.DetailsAsync(id);

            if (actor == null)
            {
                return NotFound();
            }
            var vm = new ActorsCreateUpdateViewModel();
            vm.ActorID = actor.ActorID;
            vm.FirstName = actor.FirstName;
            vm.LastName = actor.LastName;
            vm.NickName = actor.NickName;
            vm.MoviesActedFor = actor.MoviesActedFor;
            vm.Age = actor.Age;
            vm.Gender = actor.Gender;
            vm.ActorRegion = actor.ActorRegion;

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ActorsCreateUpdateViewModel vm)
        {
            var dto = new ActorsDTO()
            {
                ActorID = vm.ActorID,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                NickName = vm.NickName,
                MoviesActedFor= vm.MoviesActedFor,
                Age = vm.Age,
                Gender = vm.Gender,
                ActorRegion = vm.ActorRegion,
            };

            var result = await _actorServices.Update(dto);
            if (result == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }


        // Delete
        [HttpGet]
        public async Task<IActionResult> Delete (Guid id)
        {
            var actor = await _actorServices.DetailsAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            var vm = new ActorsDeleteViewModel();

            vm.ActorID = actor.ActorID;
            vm.FirstName = actor.FirstName;
            vm.LastName = actor.LastName;
            vm.NickName = actor.NickName;
            vm.MoviesActedFor = actor.MoviesActedFor;
            vm.Age = actor.Age;
            vm.Gender = actor.Gender;
            vm.ActorRegion = actor.ActorRegion;
            vm.EntryCreatedAt = actor.EntryCreatedAt;
            vm.EntryModifiedAt = actor.EntryModifiedAt;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var actor = await _actorServices.Delete(id);
            if (actor == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
