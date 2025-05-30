using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs.LocationDtos
{
    public class CreateClinic
    {
        [Required]
        public Guid VillageId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
    }
}