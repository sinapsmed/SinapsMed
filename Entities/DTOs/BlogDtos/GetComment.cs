using Core.Entities.DTOs;
using MailKit.Net.Pop3;

namespace Entities.DTOs.BlogDtos
{
    public class GetComment : IDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public int ReplyCount { get; set; }
    }
}