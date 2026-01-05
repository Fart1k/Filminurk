using Filminurk.Core.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class AccuWeatherController : Controller
    {
        private readonly IWeatherForecastServices _weaterForecastServices;

        public AccuWeatherController
            (
                IWeatherForecastServices weatherForecastServices
            )
        {
            _weaterForecastServices = weatherForecastServices;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
