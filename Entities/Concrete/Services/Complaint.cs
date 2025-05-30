using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete.Services
{
    public class Complaint
    {
        [Key]
        public Guid Id { get; set; }
        public Service Service { get; set; }
        public Guid ServiceId { get; set; }
        public string Title { get; set; }
    }
}