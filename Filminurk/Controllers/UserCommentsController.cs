using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.UserComments;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class UserCommentsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IUserCommentsServices _userCommentsServices;
        public UserCommentsController
            (
            FilminurkTARpe24Context context,
            IUserCommentsServices userCommentsServices
            )
        {
            _context = context;
            _userCommentsServices = userCommentsServices;
        }
        public IActionResult Index()
        {
            var result = _context.UserComments
                .Select( c => new UserCommentsIndexViewModel 
                {
                    CommentId = c.CommentId,
                    CommentBody = c.CommentBody,
                    IsHarmful = (int)c.IsHarmful,
                    CommentCreatedAt = c.CommentCreatedAt,
                }
            );
            return View(result);
        }

        // Create
        [HttpGet]
        public IActionResult NewComment()
        {
            //TODO: erista kas tegemist on admini või tavakasutajaga
            UserCommentsCreateViewModel newComment = new();
            return View(newComment);
        }

        [HttpPost, ActionName("NewComment")]
        //meetodile ei tohi panna allowanonymous

        public async Task<IActionResult> NewCommentPost(UserCommentsCreateViewModel newCommentVM)
        {
            // check DTO
            //newCommentVM.CommenterUserId = "00000000-0000-0000-000000000001";
            //TODO: newcommenti manuaalne seadmine, asenda pärast kasutaja id-ga
            Console.WriteLine(newCommentVM.CommenterUserId);
            if (ModelState.IsValid)
            {
                var dto = new UserCommentDTO() { };
                dto.CommentId = newCommentVM.CommentId;
                dto.CommentBody = newCommentVM.CommentBody;
                dto.CommenterUserId = newCommentVM.CommenterUserId;
                dto.CommentedScore = newCommentVM.CommentedScore;
                dto.CommentCreatedAt = newCommentVM.CommentCreatedAt;
                dto.CommentModifiedAt = newCommentVM.CommentModifiedAt;
                dto.CommentDeletedAt = newCommentVM.CommentDeletedAt;
                dto.IsHelpful = newCommentVM.IsHelpful;
                dto.IsHarmful = newCommentVM.IsHarmful;


                var result = await _userCommentsServices.NewComment(dto);
                if (result == null)
                {
                    return NotFound();
                }
                //TODO: erista ära kas tegu on admini või kasutajaga, admin tagastub admin-comments-index, kasutaja aga vastava filmi juurde
                return RedirectToAction(nameof(Index));
                //return RedirectToAction("Details", "Movies", id)
            }

            else { return NotFound(); }
        }
    }
}
