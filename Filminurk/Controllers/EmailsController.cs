using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Models.Emails;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class EmailsController : Controller
    {
        private readonly IEmailsServices _emailServices;

        public EmailsController(IEmailsServices emailsServices)
        {
            _emailServices = emailsServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(EmailViewModel vm)
        {
            var dto = new EmailDTO()
            {
                SendToThisAdress = vm.SendToThisAdress,
                EmailContent = vm.EmailContent,
                EmailSubject = vm.EmailSubject
            };
            _emailServices.SendEmail(dto);
            return RedirectToAction(nameof(Index));

            //Homework location


        }
    }
}
