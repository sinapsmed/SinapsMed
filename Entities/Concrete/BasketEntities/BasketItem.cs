using Core.Entities;
using Entities.Concrete.Analyses;
using Entities.Concrete.Appointments;
using Entities.Concrete.Clinics;
using Entities.Enums;

namespace Entities.Concrete.BasketEntities
{
    public class BasketItem : IEntity
    {
        public Guid Id { get; set; }
        public ItemType Type { get; set; }
        public Analysis? Analysis { get; set; }
        public Guid? AnalysisId { get; set; }
        public Appointment? Appointment { get; set; }
        public Guid? AppointmentId { get; set; }
        public int Count { get; set; }
        public Guid BasketId { get; set; }
        public Basket Basket { get; set; }
        public Guid? ClinicId { get; set; }
        public Clinic? Clinic { get; set; }
    }
}