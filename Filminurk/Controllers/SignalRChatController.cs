using Filminurk.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class SignalRChatController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        
        public SignalRChatController(SignInManager<ApplicationUser> signInManager) 
        { 
            _signInManager = signInManager; 
        }

        [Authorize]
        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}
