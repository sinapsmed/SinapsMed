namespace Entities.DTOs.SpecalitiyDtos.Create
{
    public class CreateSpecailty
    {
        public Guid CategoryId { get; set; }
        public string LogoUrl { get; set; }
        public List<SpecalityList> Data { get; set; }
    }
}