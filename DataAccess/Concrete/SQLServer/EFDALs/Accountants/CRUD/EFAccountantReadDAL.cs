using System.Net;
using Core.DataAccess;
using Core.Entities;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.AccountantEntities;
using Entities.Concrete.OrderEntities;
using Entities.DTOs.AccountantDtos;
using Entities.DTOs.AddountantDtos.Body;
using Entities.DTOs.AddountantDtos.ReturnData;
using Entities.Enums;
using Entities.Enums.Appointment;
using Entities.Enums.Payments;
using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Crypto.Modes;

namespace DataAccess.Concrete.SQLServer.EFDALs.Accountatnts.CRUD
{
    public class EFAccountantReadDAL : AccountantAdapter
    {
        private readonly IRepositoryBase<Payment, PaymentDto, AppDbContext> _repository;
        private readonly IRepositoryBase<Accountant, Read, AppDbContext> _accountantRepository;
        private readonly IRepositoryBase<OrderItem, ClinicDto, AppDbContext> _clincSale;
        private readonly IRepositoryBase<OrderItem, AppointmentDtoData, AppDbContext> _apoSale;
        public EFAccountantReadDAL(
            AppDbContext context, IRepositoryBase<Payment, PaymentDto, AppDbContext> repository,
            IStringLocalizer<AccountantAdapter> dalLocalizer,
            IRepositoryBase<Accountant, Read, AppDbContext> accountantRepository,
            IRepositoryBase<OrderItem, ClinicDto, AppDbContext> clincSale,
            IRepositoryBase<OrderItem, AppointmentDtoData, AppDbContext> apoSale) : base(context, dalLocalizer)
        {
            _repository = repository;
            _accountantRepository = accountantRepository;
            _clincSale = clincSale;
            _apoSale = apoSale;
        }

        public override async Task<IDataResult<DetailedPaymentDto>> PaymentDetail(int id)
        {
            var payment = await _context.Set<Payment>()
                .Include(c => c.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (payment is null)
                return new ErrorDataResult<DetailedPaymentDto>(_dalLocalizer["paymentNotFound"]);

            var data = AccountantSelector.DetailedPayment(payment);

            return new SuccessDataResult<DetailedPaymentDto>(data);
        }

        public override async Task<IDataResult<BaseDto<Read>>> GetAllAsync(int page, int limit)
        {
            var query = _context.Set<Accountant>()
                .Include(c => c.Email)
                .AsQueryable();

            var filter = new DtoFilter<Accountant, Read>
            {
                Limit = limit,
                Page = page,
                Selector = c => new Read
                {
                    FullName = c.Email.Title,
                    Email = c.Email.Email,
                    Id = c.Id
                }
            };

            return await _accountantRepository.GetAllAsync(query, filter);
        }

        public override async Task<IDataResult<Read>> GetByIdAsync(Guid id)
        {
            var accountant = await _context
                .Set<Accountant>()
                .Include(c => c.Email)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (accountant == null)
                return new ErrorDataResult<Read>(_dalLocalizer["AccountantNotFound"]);

            var data = new Read
            {
                FullName = accountant.Email.Title,
                Email = accountant.Email.Email,
                Id = accountant.Id
            };

            return new SuccessDataResult<Read>(data);
        }

        public override async Task<IDataResult<ClinicUperData<ClinicDto>>> ClinicSalesRecord(string? clinicKey, DateTime? start, DateTime? end, int page)
        {
            var orderItemsQuery = _context.Set<OrderItem>()
                .Where(c => c.Type == ItemType.Analysis)
                .Include(c => c.Order)
                .Include(c => c.Analysis)
                    .ThenInclude(c => c.Clinic)
                        .ThenInclude(c => c.Email)
                .Include(c => c.Analysis)
                    .ThenInclude(c => c.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(clinicKey))
                orderItemsQuery = orderItemsQuery.Where(c => c.Analysis.Clinic.UnicalKey == clinicKey);

            if (start.HasValue)
                orderItemsQuery = orderItemsQuery.Where(c => c.Order.CreatedAt >= start.Value);

            if (end.HasValue)
                orderItemsQuery = orderItemsQuery.Where(c => c.Order.CreatedAt <= end.Value);

            var orderItems = await orderItemsQuery.ToListAsync();

            var groupedClinics = orderItems
                .GroupBy(c => new
                {
                    c.Analysis.Clinic.Name,
                    c.Analysis.Clinic.Email.Email
                })
                .Select(g => new ClinicDto
                {
                    Name = g.Key.Name,
                    Email = g.Key.Email,
                    AnalysesPrice = g.Sum(x => x.Amount),
                    AnalysesFee = g.Sum(x => x.Amount * x.Count * x.Analysis.Category.DiscountedPercent / 100),
                    AnalysesPriceDiscounted = g.Sum(x => x.Amount - (x.Amount * x.Count * x.Analysis.Category.DiscountedPercent / 100)),
                    UsedAnalyses = g.Where(x => x.IsUsed).Count()
                })
                .ToList();

            double totalPrice = groupedClinics.Sum(x => x.AnalysesPrice);
            double totalFee = groupedClinics.Sum(x => x.AnalysesFee);
            double totalDiscounted = groupedClinics.Sum(x => x.AnalysesPriceDiscounted);
            double totalUsed = groupedClinics.Sum(x => x.UsedAnalyses);

            // Pagination
            int pageSize = 10;
            int totalPages = (int)Math.Ceiling((double)groupedClinics.Count / pageSize);
            var pagedData = groupedClinics
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var data = new ClinicUperData<ClinicDto>
            {
                Data = pagedData,
                CurrentPage = page,
                PageCount = totalPages,
                AnalysesFee = totalFee,
                AnalysesPrice = totalPrice,
                AnalysesPriceDiscounted = totalDiscounted,
                UsedAnalyses = (int)totalUsed
            };

            return new SuccessDataResult<ClinicUperData<ClinicDto>>(data);
        }

        public async override Task<IDataResult<AppointmentsUpperData<AppointmentDtoData>>> AppointmentsSalesRecord(Guid? expertId, DateTime? start, DateTime? end, int page)
        {
            var orderItemsQuery = _context.Set<OrderItem>()
                .Where(c => c.Type == ItemType.Appointment)
                .Include(c => c.Order)
                .Include(c => c.Appointment)
                    .ThenInclude(c => c.Expert)
                .AsQueryable();

            if (expertId.HasValue)
                orderItemsQuery = orderItemsQuery.Where(c => c.Appointment.ExpertId == expertId.Value);

            if (start.HasValue)
                orderItemsQuery = orderItemsQuery.Where(c => c.Order.CreatedAt >= start.Value);

            if (end.HasValue)
                orderItemsQuery = orderItemsQuery.Where(c => c.Order.CreatedAt <= end.Value);

            var orderItems = await orderItemsQuery.ToListAsync();

            var groupedExperts = orderItems
                .GroupBy(c => new
                {
                    ExpertId = c.Appointment.Expert.Id,
                    ExpertName = c.Appointment.Expert.FullName,
                    ExpertEmail = c.Appointment.Expert.Email
                })
                .Select(g => new AppointmentDtoData
                {
                    ExpertName = g.Key.ExpertName,
                    ExpertEmail = g.Key.ExpertEmail,
                    TotalAmount = g.Sum(x => x.Amount),
                    Fee = g.Sum(x => x.Amount * x.Count * x.Appointment.Expert.Fee / 100),
                    TotalAmountDiscounted = g.Sum(x => x.Amount - (x.Amount * x.Count * x.Appointment.Expert.Fee / 100)),
                    CompletedCount = g.Count(x => x.Appointment.Status == AppointmentStatus.Completed)
                })
                .ToList();

            double totalAmount = groupedExperts.Sum(x => x.TotalAmount);
            double totalFee = groupedExperts.Sum(x => x.Fee);
            double totalDiscounted = groupedExperts.Sum(x => x.TotalAmountDiscounted);
            int totalCompleted = groupedExperts.Sum(x => x.CompletedCount);

            int pageSize = 10;
            int totalPages = (int)Math.Ceiling((double)groupedExperts.Count / pageSize);
            var pagedData = groupedExperts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var data = new AppointmentsUpperData<AppointmentDtoData>
            {
                Data = pagedData,
                CurrentPage = page,
                PageCount = totalPages,
                CompletedCount = totalCompleted,
                TotalAmount = totalAmount,
                Fee = totalFee,
                TotalAmountDiscounted = totalDiscounted
            };

            return new SuccessDataResult<AppointmentsUpperData<AppointmentDtoData>>(data);
        }

        public override async Task<IDataResult<PaymentUperData<PaymentDto>>> Payments(
            string? orderNumber,
            string? cupon,
            DateTime? startDate,
            DateTime? endDate,
            PaymentStatus? status,
            int page,
            int limit
            )
        {

            double total = 0;
            var query = _context.Payments
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(orderNumber))
            {
                query = query.Where(x => x.UnikalKey == orderNumber);
            }
            if (!string.IsNullOrEmpty(cupon))
            {
                query = query.Where(x => x.Cupon == cupon);
            }
            if (startDate.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(x => x.CreatedAt <= endDate.Value);
            }
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            total = await query
                .SumAsync(x => x.Amount);

            var filter = new DtoFilter<Payment, PaymentDto>
            {
                Limit = limit,
                Page = page,
                Selector = AccountantSelector.Payment()
            };

            var data = await _repository.GetAllAsync(query, filter);

            var returnData = new PaymentUperData<PaymentDto>
            {
                Data = data.Data.Data,
                PageCount = data.Data.PageCount,
                CurrentPage = data.Data.CurrentPage,
                TotalAmount = total
            };

            return new SuccessDataResult<PaymentUperData<PaymentDto>>(returnData, HttpStatusCode.OK);
        }

    }
}