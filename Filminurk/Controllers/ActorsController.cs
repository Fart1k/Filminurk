using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.Actors;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class ActorsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;


        public ActorsController 
            (
                FilminurkTARpe24Context context
            )
        {
            _context = context;
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


        // Details


        // Delete
    }
}
