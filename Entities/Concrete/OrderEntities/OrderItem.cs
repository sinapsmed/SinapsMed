using Core.Entities;
using Entities.Concrete.Analyses;
using Entities.Concrete.Appointments;
using Entities.Concrete.Clinics;
using Entities.Enums;

namespace Entities.Concrete.OrderEntities
{
    public class OrderItem : IEntity
    {
        public Guid Id { get; set; }
        public ItemType Type { get; set; }
        public Analysis? Analysis { get; set; }
        public Guid? AnalysisId { get; set; }
        public Appointment? Appointment { get; set; }
        public string UnikalKey { get; set; }
        public bool IsUsed { get; set; }
        public Guid? AppointmentId { get; set; }
        public int Count { get; set; }
        public float Amount { get; set; }
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        public Clinic? Clinic { get; set; }
        public Guid? ClinicId { get; set; }
    }
}