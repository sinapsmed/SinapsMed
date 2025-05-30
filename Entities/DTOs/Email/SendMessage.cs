namespace Entities.DTOs.Email
{
    public class SendMessage
    {
        public string To { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}