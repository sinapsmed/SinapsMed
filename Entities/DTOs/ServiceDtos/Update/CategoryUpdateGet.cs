namespace Entities.DTOs.ServiceDtos.Update
{
    public class CategoryUpdateGet
    {
        public Guid Id { get; set; }
        public ICollection<CategoryUpdateLanguageGet> Languages { get; set; }
    }
}