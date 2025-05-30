using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Concrete.SQLServer.EFDALs.Appointments.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Baskets.CRUD;
using DataAccess.Concrete.SQLServer.EFDALs.Cupons.CRUD;
using Entities.Common.Payment;
using Entities.Concrete.Clinics;
using Entities.Concrete.OrderEntities;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.OrderDtos;
using Entities.DTOs.PaymentDtos;
using Entities.Enums;
using Entities.Enums.Appointment;
using Entities.Enums.Payments;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace DataAccess.Concrete.SQLServer.EFDALs.Orders.CRUD
{
    public class EFOrderCreateDAL : OrderAdapter
    {
        private readonly EFCuponUpdateDAL _cuponUpdateDAL;
        private readonly EFAppointmentCreateDAL _appointment;
        private readonly EFBasketReadDAL _basketDAL;
        private readonly IEmailService _emailService;
        private readonly EFBasketDeleteDAL _basketDeleteDAL;
        private readonly IConfiguration configuration;

        public EFOrderCreateDAL(AppDbContext context, IStringLocalizer<OrderAdapter> dalLocalizer, EFBasketReadDAL basketDAL, IConfiguration configuration, EFBasketDeleteDAL basketDeleteDAL, EFCuponUpdateDAL cuponUpdateDAL, IEmailService emailService, EFAppointmentCreateDAL appointment) : base(context, dalLocalizer)
        {
            _basketDAL = basketDAL;
            this.configuration = configuration;
            _basketDeleteDAL = basketDeleteDAL;
            _cuponUpdateDAL = cuponUpdateDAL;
            _emailService = emailService;
            _appointment = appointment;
        }

        public override async Task<IDataResult<RedirectionDto>> CheckOutOrder(string user, string? cupon)
        {
            var basket = await _basketDAL.MyBasket(user, cupon);

            if (!basket.Success)
                return new ErrorDataResult<RedirectionDto>(basket.Message);

            var totalAmount = basket.Data.Sum(c =>
            {
                return c.Count * c.Discounted;
            });

            foreach (var c in basket.Data.Where(c => c.Type == ItemType.Appointment))
            {
                var apo = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == c.ServiceId);

                if (apo is not null)
                {
                    apo.Status = AppointmentStatus.PaymentStart;
                }
            }

            if (totalAmount < 0)
                return new ErrorDataResult<RedirectionDto>(_dalLocalizer["totalAmountError"]);

            var userMod = await _context.Users.FirstOrDefaultAsync(c => c.Id == user);

            if (userMod is null)
                return new ErrorDataResult<RedirectionDto>(_dalLocalizer["userNotFound"]);

            var description = $"İstifadəçi - {userMod.FullName},Məbləğ - {totalAmount}";

            IDataResult<RedirectionDto> redUrl;

            var payment = new Payment
            {
                Amount = totalAmount,
                CreatedAt = DateTime.UtcNow,
                Currency = "AZN",
                Description = description,
                Language = "az",
                Status = PaymentStatus.Pending,
                UserId = userMod.Id,
                UnikalKey = OrderServices.GenerateUniqueUnicalKeyForPayment(_context),
                Cupon = cupon,
                PaymentId = -1,
            };

            await _context.Set<Payment>().AddAsync(payment);

            await _context.SaveChangesAsync();

            if (totalAmount > 0)
            {
                redUrl = await CreateOrder(totalAmount, description);

                payment.PaymentId = redUrl.Data.PaymentId;

                if (!redUrl.Success)
                    return new ErrorDataResult<RedirectionDto>(redUrl.Message);
            }
            else
            {
                var paymentResponse = await Payment(-1, PaymentStatus.FullyPaid, payment.UnikalKey);

                return new SuccessDataResult<RedirectionDto>(paymentResponse.Message, paymentResponse.StatusCode);
            }
            
            await _context.SaveChangesAsync();

            return redUrl;
        }

        public override async Task<IResult> Payment(int paymentId, PaymentStatus status, string? unikalKey = null)
        {
            var payment = await _context.Set<Payment>()
                .FirstOrDefaultAsync(c => c.PaymentId == paymentId);

            if (unikalKey != null && paymentId == -1)
            {
                payment = await _context.Set<Payment>()
                    .FirstOrDefaultAsync(c => c.PaymentId == paymentId && c.UnikalKey == unikalKey);
            }

            if (payment is null)
                return new ErrorResult(_dalLocalizer["paymentNotFound"]);

            var user = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == payment.UserId);

            if (user is null)
                return new ErrorResult(_dalLocalizer["userNotFound"]);

            var userBasket = await _basketDAL.MyBasket(user.Id, payment.Cupon);

            if (!userBasket.Success)
                return new ErrorResult(userBasket.Message);

            payment.Status = status;

            if (status is PaymentStatus.FullyPaid)
            {
                var clinicIds = userBasket.Data
                    .Select(c => c.ClinicDetail?.Id)
                    .Where(id => id != null)
                    .Distinct()
                    .ToList();

                var clinics = await _context.Set<Clinic>()
                    .Where(c => clinicIds.Contains(c.Id))
                    .ToDictionaryAsync(c => c.Id);


                var orderItems = userBasket.Data.Select(c => new OrderItem
                {
                    Amount = c.Discounted,
                    Count = c.Count,
                    UnikalKey = OrderServices.GenerateUniqueUnicalKey(),
                    Type = c.Type,
                    AnalysisId = OrderServices.GetCorrectId(c, ItemType.Analysis),
                    AppointmentId = OrderServices.GetCorrectId(c, ItemType.Appointment),
                    ClinicId = c.ClinicDetail?.Id,
                    Clinic = c.ClinicDetail?.Id != null && clinics.TryGetValue(c.ClinicDetail.Id, out var clinic)
                        ? clinic
                        : null,
                    IsUsed = false,
                }).ToList();


                var order = new Order
                {
                    CreatedAt = DateTime.UtcNow,
                    UserId = user.Id,
                    Items = orderItems.ToList(),
                };
                var orderItemDtos = order.Items.Select(item => new OrderItemDto
                {
                    Amount = item.Amount,
                    Count = item.Count,
                    UnikalKey = item.UnikalKey,
                    Type = item.Type,
                    AnalysisId = item.AnalysisId,
                    AppointmentId = item.AppointmentId,
                    ClinicId = item.ClinicId,
                    Clinic = item.Clinic != null ? new ClinicDto
                    {
                        Id = item.Clinic.Id,
                        Name = item.Clinic.Name,
                        UnicalKey = item.Clinic.UnicalKey,
                        Analyses = item.Clinic.Analyses?.Select(a => new AnalysisDto
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Category = a.Category != null
                                ? new CategoryDto
                                {
                                    Id = a.Category.Id,
                                    Name = a.Category.Name
                                }
                                : null
                        }).ToList() ?? new()
                    } : null
                }).ToList();

                var orderDto = new OrderDto
                {
                    CreatedAt = order.CreatedAt,
                    UserId = order.UserId,
                    Items = orderItemDtos
                }; 
                if (payment.Cupon is not null)
                {
                    BackgroundJob.Enqueue(() => _cuponUpdateDAL.UseCupon(user.Id, payment.Cupon, payment.Amount));
                }

                BackgroundJob.Enqueue(() => SendInvoice(orderDto, user.FullName, user.Email));
                BackgroundJob.Enqueue(() => _basketDeleteDAL.RemoveAll(user.Id));

                var apointmentIds = order.Items
                    .Where(c => c.Type is ItemType.Appointment && c.AppointmentId is not null)
                    .Select(c => new PaymentVerifiedForAppointment
                    {
                        Id = c.AppointmentId.Value,
                        Price = c.Amount
                    })
                    .ToList();

                if (apointmentIds.Count > 0)
                    BackgroundJob.Enqueue(() => _appointment.AppointmentsPaid(apointmentIds));

                foreach (var item in order.Items)
                {
                    item.Clinic = null;
                }
                await _context.Set<Order>().AddAsync(order);

            }
            else
            {
                if (!userBasket.Success)
                    return new ErrorDataResult<RedirectionDto>(userBasket.Message);

                foreach (var c in userBasket.Data.Where(c => c.Type == ItemType.Appointment))
                {
                    var apo = await _context.Appointments
                        .FirstOrDefaultAsync(x => x.Id == c.ServiceId);

                    if (apo is not null)
                    {
                        apo.Status = AppointmentStatus.Pending;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return new SuccessResult(_dalLocalizer["paymentprocess", status]);
        }

        private async Task<IDataResult<RedirectionDto>> CreateOrder(float amount, string description)
        {
            using (var client = new HttpClient())
            {
                var authToken = Encoding.ASCII.GetBytes($"{configuration["BankAcconut:UserName"]}:{configuration["BankAcconut:Password"]}");
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));


                var bodyDataUrl = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PaymentBody.json");
                var bodyData = File.ReadAllText(bodyDataUrl);

                bodyData = bodyData
                    .Replace("{{amount}}", amount.ToString().Replace(',', '.'))
                    .Replace("{{description}}", description)
                    .Replace("{{hppRedirectUrl}}", configuration["App:BaseLink"] + "/payment");

                HttpContent content = new StringContent(bodyData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"{configuration["BankAcconut:EnvUrl"]}api/order", content);

                string responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorDataResult<RedirectionDto>("Ödəniş zamanı problem baş verdi", HttpStatusCode.BadRequest, responseBody);
                }

                var dataJson = JsonConvert.DeserializeObject<PaymentOrderBase>(responseBody);

                var redurl = $"{dataJson?.Order.HppUrl}?id={dataJson?.Order.Id}&password={dataJson?.Order.Password}";

                return new SuccessDataResult<RedirectionDto>(new RedirectionDto { RedirectionUrl = redurl, PaymentId = dataJson.Order.Id }, HttpStatusCode.Redirect);
            }
        }

        public async Task SendInvoice(OrderDto order, string user, string email)
        {
            var url = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "invoice.html");
            string invoiceTemplate = File.ReadAllText(url);

            StringBuilder basketItemsHtml = new StringBuilder();

            foreach (var item in order.Items)
            {
                basketItemsHtml.AppendLine("<tr>");
                basketItemsHtml.AppendLine($"<td>{await ItemTitle(item)}</td>");
                basketItemsHtml.AppendLine($"<td>{item.Count}</td>");
                basketItemsHtml.AppendLine($"<td>{item.Amount.ToString("0.00")}</td>");
                basketItemsHtml.AppendLine($"<td>{item.UnikalKey}</td>");
                basketItemsHtml.AppendLine($"<td>{(item.Count * item.Amount).ToString("0.00")}</td>");

                if (item.Clinic != null)
                {
                    basketItemsHtml.AppendLine("<td>");
                    basketItemsHtml.AppendLine($"<strong>Ad:</strong> {item.Clinic.Name}<br>");
                    basketItemsHtml.AppendLine($"<strong>Açar:</strong> {item.Clinic.UnicalKey}<br>");
                    basketItemsHtml.AppendLine("</td>");
                }
                else
                {
                    basketItemsHtml.AppendLine("<td></td>");
                }

                basketItemsHtml.AppendLine("</tr>");
            }

            StringBuilder invoiceBuilder = new StringBuilder(invoiceTemplate);
            invoiceBuilder
                .Replace("{{greeting}}", $"Salam {user}")
                .Replace("{{BasketItems}}", basketItemsHtml.ToString());

            await _emailService.SendEmailAsync(email, "Sifarişinizin faturası", invoiceBuilder.ToString());
        }

        private async Task<string> ItemTitle(OrderItemDto item)
        {
            if (item.Type is ItemType.Analysis)
            {
                var analysis = await _context.Analyses.FirstOrDefaultAsync(c => c.Id == item.AnalysisId);
                return $"{analysis?.Name} ({analysis?.Code})";
            }
            else if (item.Type is ItemType.Appointment)
            {
                return $"Randevu";
            }
            else
            {
                return "Something was wrong...";
            }
        }
    }
}