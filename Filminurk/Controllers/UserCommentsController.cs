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

            return NotFound();
        }

        //Details
        [HttpGet]
        public async Task<IActionResult> DetailsAdmin(Guid id)
        {
            var requestedComment = await _userCommentsServices.DetailAsync(id);

            if (requestedComment == null) { return NotFound(); }

            var commentVM = new UserCommentsIndexViewModel();

            commentVM.CommentId = requestedComment.CommentId;
            commentVM.CommentBody = requestedComment.CommentBody;
            commentVM.CommenterUserId = requestedComment.CommenterUserId;
            commentVM.CommentedScore = requestedComment.CommentedScore;
            commentVM.CommentCreatedAt = requestedComment.CommentCreatedAt;
            commentVM.CommentModifiedAt = requestedComment.CommentModifiedAt;
            commentVM.CommentDeletedAt = requestedComment.CommentDeletedAt;

            return View(commentVM);
        }

        //Delete
        [HttpGet]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var deleteEntry = await _userCommentsServices.DetailAsync(id);
            if (deleteEntry == null) { return NotFound(); }

            var commentVM = new UserCommentsIndexViewModel();
            commentVM.CommentId = deleteEntry.CommentId;
            commentVM.CommentBody = deleteEntry.CommentBody;
            commentVM.CommenterUserId = deleteEntry.CommenterUserId;
            commentVM.CommentedScore = deleteEntry.CommentedScore;
            commentVM.CommentCreatedAt = deleteEntry.CommentCreatedAt;
            commentVM.CommentModifiedAt = deleteEntry.CommentModifiedAt;
            commentVM.CommentDeletedAt = deleteEntry.CommentDeletedAt;
            return View("DeleteAdmin", commentVM);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCommentPost(Guid id)
        {
            var deleteThisComment = await _userCommentsServices.Delete(id);
            if (deleteThisComment == null) { return NotFound(); }
            return RedirectToAction("Index");
        }
    }
}
