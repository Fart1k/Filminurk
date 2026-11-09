namespace Filminurk.Core.Dto
{
    public class FileToDatabaseDTO
    {
        public Guid ImageId { get; set; }
        public required string ImageTitle { get; set; }
        public required byte[] ImageData { get; set; }
        public required string Image { get; set; }
        public Guid? ListId { get; set; }
    }
}
