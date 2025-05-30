using Core.Entities.DTOs;
using Entities.Concrete.Forms.Enum;

namespace Entities.DTOs.AppointmentsDtos.GetData
{
    public class AppointmentFormData : IDto
    {
        public string ServiceName { get; set; }
        public string ExpertFullName { get; set; }
        public UserDetail User { get; set; }
        public int Number { get; set; }
        public string Date { get; set; }
        public AgeRange AgeRange { get; set; }
        public ICollection<string> Complaints { get; set; }
    }

    public class UserDetail
    {
        public string FullName { get; set; }
        public string UnicalKey { get; set; }
        public string DateOfBrith { get; set; }
        public bool Gender { get; set; }
    }
}