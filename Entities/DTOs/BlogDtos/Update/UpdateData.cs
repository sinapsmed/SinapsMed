namespace Entities.DTOs.BlogDtos.Update
{
    public class UpdateData
    {
        public Guid? Updater { get; set; }
        public Guid BlogId { get; set; }
        public string ImageUrl { get; set; }
        public List<UpdateLang> Languages { get; set; }
    }
}