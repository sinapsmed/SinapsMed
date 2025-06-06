using System.Net;
using Core.DataAccess;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Core.Utilities.Static;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Concrete.SQLServer.EFDALs.Baskets.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Common;
using DataAccess.Concrete.SQLServer.EFDALs.Experts.CRUD;
using DataAccess.Services.Abstract;
using Entities.Concrete.Appointments;
using Entities.Concrete.BasketEntities;
using Entities.Concrete.Experts.WorkTimes;
using Entities.Concrete.Forms;
using Entities.Concrete.Forms.Diagnoses;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.BasketDtos.BodyDtos;
using Entities.Enums;
using Entities.Enums.Appointment;
using Google.Apis.Calendar.v3.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Appointments.CRUD
{
    public class EFAppointmentCreateDAL : AppointmentAdapter
    {
        private readonly IStringLocalizer<CommonLocalizer> _comLoclizer;
        private readonly EFBasketDeleteDAL _basketDelete;
        private readonly EFExpertCustomDAL _expert;
        private readonly EFBasketCreateDAL _basket;
        private readonly IGoogleService _google;
        private readonly IEmailService _email;
        private readonly IRepositoryBase<Appointment, Create, AppDbContext> _repo;
        public EFAppointmentCreateDAL(
            AppDbContext context,
            IStringLocalizer<AppointmentAdapter> localizer,
            IRepositoryBase<Appointment, Create, AppDbContext> repo,
            EFBasketCreateDAL basket,
            IStringLocalizer<CommonLocalizer> comLoclizer,
            EFBasketDeleteDAL basketDelete,
            EFExpertCustomDAL expert,
            IEmailService email,
            IGoogleService google) : base(context, localizer)
        {
            _repo = repo;
            _basket = basket;
            _comLoclizer = comLoclizer;
            _basketDelete = basketDelete;
            _expert = expert;
            _email = email;
            _google = google;
        }

        public override async Task<IResult> AddAnamnezFormDiagnosis(AnamnezCreate create)
        {
            var appointment = await _context.Set<Appointment>()
                .Include(c => c.Form)
                    .ThenInclude(c => c.AnamnezFormDiagnoses)
                .FirstOrDefaultAsync(c => c.Id == create.AppointmentId);

            if (appointment is null)
                return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            if (appointment.Form is not null)
                return new ErrorResult(_dalLocalizer["anamnezFormAlreadyExists"], HttpStatusCode.BadRequest);

            var anamnezForm = create.Map<AnamnezForm, AnamnezCreate>();

            anamnezForm.AnamnezFormDiagnoses = new List<AnamnezFormDiagnosis>();

            foreach (var item in create.AnamnezFormDiagnoses)
            {
                anamnezForm.AnamnezFormDiagnoses.Add(new AnamnezFormDiagnosis
                {
                    DiagnosisId = item.DiagnosisId,
                    Type = item.Type
                });
            }

            await _context.Set<AnamnezForm>().AddAsync(anamnezForm);
            await _context.SaveChangesAsync();

            return new SuccessResult(_dalLocalizer["anamnezFormAdded"]);
        }

        public override async Task<IResult> Create(Create create)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == create.UserId);

            if (user is null)
                return new ErrorResult(_dalLocalizer["userNotFound"], HttpStatusCode.NotFound);

            var expert = await _context.Experts
                    .FirstOrDefaultAsync(c => c.Id == create.ExpertId);
            if (expert is null)
                return new ErrorResult(_dalLocalizer["expertNotFound"], HttpStatusCode.NotFound);


            if (!await _expert.IsWorkingTime(expert.Id, create.Date))
                return new ErrorResult(_dalLocalizer["expertBronedTime"], HttpStatusCode.NotFound);

            var servicePeriod = await _context.ServicePeriods
                    .FirstOrDefaultAsync(c => c.Id == create.ServicePeriodId);

            if (servicePeriod is null)
                return new ErrorResult(_dalLocalizer["servicePeriodNotFound"], HttpStatusCode.NotFound);

            var appointment = create.Map<Appointment, Create>();

            if (create.AppointmentType is not AppointmentType.MySelf)
            {
                if (create.AdditionalUser is null)
                    return new ErrorResult(_dalLocalizer["additionalUserNull"], HttpStatusCode.BadRequest);

                appointment.AdditionalUser = create.AdditionalUser.Map<AdditionalUser, AdditionalUserDto>();
            }
            bool isSucces = true;
            string UnicalKey = "";
            do
            {
                UnicalKey = AppointmentService.GenerateUniqueUnicalKey(_context);
                appointment.UnicalKey = UnicalKey;
                try
                {
                    await _context.Set<Appointment>().AddAsync(appointment);
                    await _context.SaveChangesAsync();
                    isSucces = false;
                }
                catch (DbUpdateException ex)
                {
                    UnicalKey = AppointmentService.GenerateUniqueUnicalKey(_context);
                }
                catch (Exception ex)
                {
                    return new ErrorResult(message: _comLoclizer["ex"], statusCode: HttpStatusCode.BadRequest, ex);
                }
            } while (isSucces);

            var apointment = await _context.Set<Appointment>().FirstOrDefaultAsync(c => c.UnicalKey == UnicalKey);

            if (apointment is null)
                return new ErrorResult(message: _dalLocalizer["apoNotFound", UnicalKey], statusCode: HttpStatusCode.NotFound);

            var addItem = await _basket.AddItem(new AddItem
            {
                AppointmentId = apointment.Id,
                Count = 1,
                Type = ItemType.Appointment,
                UserId = apointment.UserId
            });

            if (!addItem.Success)
                return addItem;

            var bron = await _expert.BronExpertTime(expert.Id, apointment.Date, TimeSpan.FromMinutes(servicePeriod.Duration));

            if (!bron.Success)
                return bron;

            BackgroundJob.Enqueue<EFAppointmentCreateDAL>(x => x.WaitPatment(appointment.Id));

            return new SuccessResult();
        }

        public async override Task<IResult> AddFile(string filePath, string title, Guid appointmentId)
        {
            var appointment = await _context.Set<Appointment>()
                .Include(c => c.Attachments)
                .FirstOrDefaultAsync(c => c.Id == appointmentId);

            if (appointment is null)
                return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound);

            appointment.Attachments
                .Add(new AppointmentAttachment
                {
                    Attachment = filePath,
                    Title = title,
                });

            await _context.SaveChangesAsync();

            return new SuccessResult();
        }


        [Queue("high")]
        public async Task AppointmentsPaid(List<PaymentVerifiedForAppointment> payments)
        {
            foreach (var payment in payments)
            {
                var appointment = await _context.Set<Appointment>()
                    .Include(c => c.ServicePeriod)
                        .ThenInclude(c => c.Languages)
                    .Include(c => c.User)
                    .Include(c => c.Expert)
                    .Include(c => c.AdditionalUser)
                    .FirstOrDefaultAsync(c => c.Id == payment.Id);

                if (appointment is null)
                    continue;

                appointment.Price = payment.Price;

                appointment.Status = AppointmentStatus.Confirmed;

                var workPause = await _context.Set<WorkPause>()
                    .FirstOrDefaultAsync(c => c.ExpertId == appointment.ExpertId && c.Start == appointment.Date);

                if (workPause is not null)
                {
                    workPause = new WorkPause
                    {
                        ExpertId = appointment.ExpertId,
                        Start = appointment.Date,
                        End = appointment.Date.AddMinutes(appointment.ServicePeriod.Duration),
                        Reason = _dalLocalizer["reservedTime"]
                    };
                    await _context.Set<WorkPause>().AddAsync(workPause);
                }
                else
                {
                    workPause.Reason = _dalLocalizer["reservedTime"];
                }

                var meetLinkService = await _google.CreateScheduledMeet(
                    new List<EventAttendee> {
                        new EventAttendee { Email = appointment.User.Email},
                        new EventAttendee { Email = appointment.Expert.Email}
                    }, appointment.Date, appointment.Date.AddMinutes(appointment.ServicePeriod.Duration), $"{appointment.Expert.FullName} ilə görüş", appointment.Expert.Email);

                var meetLink = meetLinkService.Data;

                appointment.MeetLink = meetLink;

                await _context.SaveChangesAsync();

                var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "appointment.html");

                string fileContent = File.ReadAllText(url);

                fileContent = fileContent
                    .Replace("{{Number}}", appointment.Number.ToString())
                    .Replace("{{Date}}", appointment.Date.ToString("dd/MM/yyyy HH:mm"))
                    .Replace("{{Status}}", appointment.Status.ToString())
                    .Replace("{{ServicePeriod}}", appointment.ServicePeriod.Languages.FirstOrDefault(c => c.Code == "az")?.PeriodText)
                    .Replace("{{AppointmentType}}", AppointmentService.AppointmentTypeSwithc(appointment.AppointmentType))
                    .Replace("{{UnicalKey}}", appointment.UnicalKey)
                    .Replace("{{UserNote}}", appointment.UserNote ?? string.Empty)
                    .Replace("{{MeetLink}}", appointment.MeetLink)
                    .Replace("{{UserName}}", appointment.User.FullName)
                    .Replace("{{UserEmail}}", appointment.User.Email)
                    .Replace("{{UserWhatsApp}}", appointment.UserWhatsApp)
                    .Replace("{{ExpertName}}", appointment.Expert.FullName)
                    .Replace("{{MeetLink}}", meetLink)
                    .Replace("{{phoneNum}}", appointment.Expert.ShowPhone ? appointment.Expert.PhoneNumber : "")
                    .Replace("{{ExpertEmail}}", appointment.Expert.Email);


                await _email.SendBulkEmailAsync(new List<string> { appointment.User.Email, appointment.Expert.Email }, _dalLocalizer["apoConfirmSub"], fileContent);
            }
        }

        [Queue("high")]
        public async Task WaitPatment(Guid appointmentId)
        {
            await Task.Delay(TimeSpan.FromMinutes(15));

            var appointment = await _context.Set<Appointment>().FirstOrDefaultAsync(c => c.Id == appointmentId);

            if (appointment.Status is AppointmentStatus.PaymentStart)
            {
                await WaitPatment(appointmentId);
            }

            if (appointment.Status is AppointmentStatus.Pending)
            {
                var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == appointment.UserId);
                var userBasket = await _context.Set<Basket>()
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserId == appointment.UserId);
                if (userBasket is null)
                    throw new Exception("Basket Tapilmadi");

                var item = userBasket.Items.FirstOrDefault(c => c.AppointmentId == appointment.Id);

                if (item is null)
                    throw new Exception("Appointment not Found In user Basket");

                await _basketDelete.RemoveItem(appointment.UserId, item.Id);

                await _expert.CancelBronExpertTime(appointment.ExpertId, appointment.Date);

                var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "appointmentCancel.html");

                string fileContent = File.ReadAllText(url);

                fileContent = fileContent
                    .Replace("{{UserName}}", user.FullName)
                    .Replace("{{Number}}", appointment.Number.ToString())
                    .Replace("{{Date}}", appointment.Date.ToString("dd/MM/yyyy HH:mm"));


                await _email.SendEmailAsync(user.Email, "Sifariş Ləğv Edildi", fileContent);

                _context.Appointments.Remove(appointment);

                await _context.SaveChangesAsync();
            }
        }
    }
}