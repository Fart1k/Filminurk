using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class OmdbController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
