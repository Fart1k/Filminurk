namespace Filminurk.Core.Dto.AccountsDTOs
{
    public class ApplicationUsersDTOs
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool ProfileType { get; set; }
        public List<Guid>? FavoriteListsIds { get; set; }
        public List<Guid>? CommentIds { get; set; }
        public string? AvatarImageId { get; set; }
        public string DisplayName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
    }
}
