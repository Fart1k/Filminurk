namespace Filminurk.Core.Dto
{
    public class EmailDTO
    {
        public required string SendToThisAdress { get; set; }
        public required string EmailSubject { get; set; }
        public required string EmailContent { get; set; }
    }
}
