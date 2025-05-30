using Core.Entities;

namespace Entities.Concrete.Appointments
{
    public class AdditionalUser : IEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBrith { get; set; }
        public bool Gender { get; set; }
    }
}