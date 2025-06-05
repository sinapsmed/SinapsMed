using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Entities.Concrete.Appointments;
using Entities.Concrete.Experts.WorkTimes;
using Entities.Concrete.Services;

namespace Entities.Concrete.Experts
{
    public class Expert : IEntity
    {
        public Guid Id { get; set; }
        public string Specality { get; set; }
        public string Resume { get; set; }
        public bool Boosted { get; set; }
        public string PasswordHash { get; set; }
        //With %
        public byte Fee { get; set; }
        public string PhotoUrl { get; set; }
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool ShowPhone { get; set; }
        public string? RefleshToken { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public WorkRoutine Routine { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuspend { get; set; } = false;
        public ICollection<Service> Services { get; set; } = new List<Service>();
        public List<ExpertServicePeriod> ServicePeriods { get; set; } = new List<ExpertServicePeriod>();
        public List<Appointment>? Appointments { get; set; } = new List<Appointment>();
        public List<WorkHoliday> Holidays { get; set; } = new List<WorkHoliday>();
        public ICollection<WorkPause> Pauses { get; set; } = new List<WorkPause>();
    }
}