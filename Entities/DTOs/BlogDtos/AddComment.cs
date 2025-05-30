namespace Entities.DTOs.BlogDtos
{
    public class AddComment
    {
        public Guid BlogId { get; set; }
        public Guid? ParentId { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
    }
}