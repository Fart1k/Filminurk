using Microsoft.AspNetCore.Identity;

namespace Filminurk.Core.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public List<Guid>? FavoriteListsIds { get; set; }
        public List<Guid>? CommentIds { get; set; }
        public string AvatarImageId { get; set; }
        public string DisplayName { get; set; }
        public bool ProfileType { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }

    }
}
