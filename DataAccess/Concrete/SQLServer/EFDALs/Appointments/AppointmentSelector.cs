using System.Linq.Expressions;
using Entities.Concrete.Appointments;
using Entities.DTOs.AppointmentsDtos.GetData;
using Entities.DTOs.ExpertDtos.GetDtos;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.Users;
using Entities.Enums;

namespace DataAccess.Concrete.SQLServer.EFDALs.Appointments
{
    public static class AppointmentSelector
    {
        private static string AppointmentTitle(Superiority superiority, Appointment appointment, string titleJoin)
        {
            if (superiority == Superiority.Expert)
                return titleJoin + appointment.User.FullName;
            else if (superiority == Superiority.User)
                return titleJoin + $"{appointment.Expert.Specality} {appointment.Expert.FullName}";
            else
                return "Unexpected";
        }
        public static Expression<Func<Appointment, AppointmentSchedule>> Schedule(string titleJoin, Superiority superiority)
        {
            return c => new AppointmentSchedule
            {
                End = c.Date.AddMinutes(c.ServicePeriod.Duration),
                Start = c.Date,
                Title = AppointmentTitle(superiority, c, titleJoin)
            };
        }
        public static Expression<Func<Appointment, AppointmentList>> ListAppointment(string _cultre)
        {
            return c => new AppointmentList
            {
                Id = c.Id,
                Date = c.Date,
                Expert = new ExpertDetail
                {
                    Email = c.Expert.Email,
                    FullName = $"{c.Expert.Specality} {c.Expert.FullName}",
                    PhotoUrl = c.Expert.PhotoUrl
                },
                Service = new PeriodGetDto
                {
                    Duration = c.ServicePeriod.Duration,
                    Id = c.ServicePeriod.Id,
                    Title = c.ServicePeriod.Service.Languages.FirstOrDefault(c => c.Code == _cultre).Title
                },
                Status = c.Status,
                User = new DetailedUser
                {
                    Email = c.User.Email,
                    FullName = c.User.FullName,
                    Gender = c.User.Gender,
                    Id = c.User.Id,
                    ImageUrl = c.User.ImageUrl
                },
                Number = c.Number
            };
        }

        public static AppointmentDetail Detail(this Appointment c, string _cultre)
        {
            return new()
            {
                Id = c.Id,
                Date = c.Date,
                Expert = new ExpertDetail
                {
                    Email = c.Expert.Email,
                    FullName = $"{c.Expert.Specality} {c.Expert.FullName}",
                    PhotoUrl = c.Expert.PhotoUrl
                },
                Service = new PeriodGetDto
                {
                    Duration = c.ServicePeriod.Duration,
                    Id = c.ServicePeriod.Id,
                    Title = c.ServicePeriod.Service.Languages.FirstOrDefault(c => c.Code == _cultre).Title
                },
                Status = c.Status,
                AnamnezFormId = c.Form is null ? null : c.Form.Id,
                User = new DetailedUser
                {
                    Email = c.User.Email,
                    FullName = c.User.FullName,
                    Gender = c.User.Gender,
                    Id = c.User.Id,
                    ImageUrl = c.User.ImageUrl
                },
                AppointmentType = c.AppointmentType,
                Attachments = c.Attachments.Select(c => c.Attachment),
                MeetLink = c.MeetLink,
                UserNote = c.UserNote,
                UserWhatsApp = c.UserWhatsApp,
                Duration = c.ServicePeriod.Duration,
                Number = c.Number,
            };
        }
    }
}