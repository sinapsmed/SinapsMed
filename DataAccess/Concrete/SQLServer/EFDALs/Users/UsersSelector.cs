using System.Linq.Expressions;
using DataAccess.Services;
using Entities.Concrete.Appointments;
using Entities.Concrete.OrderEntities;
using Entities.Concrete.UserEntities;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.ClinicDtos.DataDtos;
using Entities.DTOs.Users;

namespace DataAccess.Concrete.SQLServer.EFDALs.Users
{
    public class UsersSelector
    {
        public static Expression<Func<AppUser, DetailedUser>> Users()
        {
            return c => new DetailedUser
            {
                Email = c.Email,
                FullName = c.FullName,
                Gender = c.Gender,
                Id = c.Id,
                ImageUrl = c.ImageUrl,
                UnikalId = c.UnicalKey,
                DateOfBrith = c.DateOfBrith == DateTime.MinValue ? null : c.DateOfBrith
            };
        }

        public static Expression<Func<Appointment, UserAppointment>> UserAppointments()
        {
            return c => new UserAppointment
            {
                Id = c.Id,
                ExpertPhone = c.Expert.ShowPhone ? c.Expert.PhoneNumber : null,
                Date = c.Date,
                ExpertName = $"{c.Expert.Specality} {c.Expert.FullName}",
                ExpertImageUrl = c.Expert.PhotoUrl,
                Price = c.Price,
                UnikalKey = c.UnicalKey
            };
        }
        public static UserAppointmentDetailed UserAppointmentDetailed(Appointment appointment, string _cultre)
        {
            return new UserAppointmentDetailed
            {
                Id = appointment.Id,
                UserNote = appointment.UserNote,
                ServiceName = appointment.ServicePeriod.Service.Languages.FirstOrDefault(c => c.Code == _cultre)?.Title,
                ServiceId = appointment.ServicePeriod.ServiceId,
                ExpertName = $"{appointment.Expert.Specality} {appointment.Expert.FullName}",
                ExpertImageUrl = appointment.Expert.PhotoUrl,
                UserWhatsApp = appointment.UserWhatsApp,
                MeetLink = appointment.MeetLink,
                Duration = appointment.ServicePeriod.Duration,
                Price = appointment.Price,
                UnikalKey = appointment.UnicalKey,
                Date = appointment.Date,
                Status = appointment.Status,
                AdditionalUser = appointment.AdditionalUser is null ? null : new AdditionalUserDto
                {
                    DateOfBrith = appointment.AdditionalUser.DateOfBrith,
                    FullName = appointment.AdditionalUser.FullName,
                    Gender = appointment.AdditionalUser.Gender
                },
                AppointmentType = appointment.AppointmentType,
                Attachments = appointment.Attachments.Select(c => new AppointmentAttachmentDto
                {
                    Attachment = c.Attachment,
                    Title = c.Title
                }).ToList()
            };
        }

        public static Expression<Func<OrderItem, GetUserAnlysis>> UserAnalysis()
        {
            return c => new GetUserAnlysis
            {
                Code = c.Analysis.Code,
                Id = c.Analysis.Id,
                Name = c.Analysis.Name,
                IsVerificated = c.IsUsed,
                Date = c.Order.CreatedAt,
                Price = c.Amount,
                UnikalKey = c.UnikalKey,
                Count = c.Count,
                Clinic = new ClinicBasketDetail
                {
                    Id = c.Clinic.Id,
                    MapLink = EFService.MapUrl(c.Clinic.Latitude, c.Clinic.Longitude),
                    Name = c.Clinic.Name,
                    UnicalKey = c.Clinic.UnicalKey
                }
            };
        }
    }
}