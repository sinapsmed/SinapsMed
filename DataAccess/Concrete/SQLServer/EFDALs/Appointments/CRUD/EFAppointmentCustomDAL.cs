using System.Text;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Appointments;
using Entities.Concrete.Forms.Diagnoses;
using ExcelDataReader;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Appointments.CRUD
{
    public class EFAppointmentCustomDAL : AppointmentAdapter
    {
        private readonly IEmailService _email;
        public EFAppointmentCustomDAL(
            AppDbContext context,
            IStringLocalizer<AppointmentAdapter> dalLocalizer,
            IEmailService email) : base(context, dalLocalizer)
        {
            _email = email;
        }

        public async override Task<IResult> UploadDiagnostics(string fileName)
        {
            if (await _context.Set<Diagnosis>().AnyAsync())
                return new ErrorResult(_dalLocalizer["alreadyUploaded"]);

            BackgroundJob.Enqueue<EFAppointmentCustomDAL>(x => x.UploadBackgorundTask(fileName));

            return new SuccessResult("Backgorund Job Has been started");
        }

        public override async Task<IResult> NotifyExpert(Guid id)
        {
            var appointment = await _context
                .Set<Appointment>()
                .Include(c => c.User)
                .Include(c => c.Expert)
                .Include(c => c.Attachments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (appointment is null)
                return new ErrorResult(_dalLocalizer["notFound"]);

            var uri = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "Mail", "notify.html");
            var message = await File.ReadAllTextAsync(uri);

            message = message
                .Replace("{{head}}", $"{appointment.User.FullName}")
                .Replace("{{info}}",
                    $"Randevu Nömrəsi : {appointment.Number} </br>Randevu Tarixi : {appointment.Date.ToString("G")}</br>Yüklənən sənəd sayı : {appointment.Attachments.Count()}</br>Istifadəçi UnikalId : {appointment.User.UnicalKey}");

            await _email.SendEmailAsync(appointment.Expert.Email, "Pasiyent Sənədləri Hazırladı", message);

            return new SuccessResult();
        }

        [Queue("low")]
        public async Task UploadBackgorundTask(string fileName)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ICollection<Diagnosis> diagnoses = new List<Diagnosis>();

            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int rowindex = 0;
                    while (reader.Read())
                    {
                        if (rowindex == 0)
                        {
                            rowindex++;
                            continue;
                        }
                        diagnoses.Add(new Diagnosis
                        {
                            ICD10_Code = reader.GetString(0),
                            WHO_Full_Desc = reader.GetString(1),
                        });
                    }
                }
            }
            try
            {
                await _context.Set<Diagnosis>().AddRangeAsync(diagnoses);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                await _email.SendEmailAsync("sinapsmed.teletibb@gmail.com", "File Uploaded", "File Uploaded Succes");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                await _email.SendEmailAsync("sinapsmed.teletibb@gmail.com", "File Uploaded", "File Upload failed exception is : " + ex.Message);
            }
            finally
            {
                File.Delete(fileName);
            }
        }
    }
}