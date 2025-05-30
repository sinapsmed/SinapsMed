namespace Entities.DTOs.SpecalitiyDtos.Create
{
    public class CreateComplaint
    {
        public Guid ServiceId { get; set; }
        public IEnumerable<string> Complaints { get; set; }
    }
}