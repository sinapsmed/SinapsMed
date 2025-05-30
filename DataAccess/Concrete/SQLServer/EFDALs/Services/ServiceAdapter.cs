using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Services;
using Entities.DTOs.Helpers;
using Entities.DTOs.ServiceDtos.Create;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.ServiceDtos.Update;
using Entities.DTOs.SpecalitiyDtos.Create;
using Entities.DTOs.SpecalitiyDtos.Get;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Services
{
    public class ServiceAdapter : BaseAdapter, IServiceDAL
    {
        protected readonly IRepositoryBase<Service, GetSpecality, AppDbContext> _servRepo;
        protected readonly IRepositoryBase<Service, GetService, AppDbContext> _servAdm;
        protected readonly IRepositoryBase<ServicePeriod, PeriodGetDto, AppDbContext> _periodGServ;
        protected readonly IRepositoryBase<ServiceCategory, GetCat, AppDbContext> _catRepo;
        protected readonly IStringLocalizer<ServiceAdapter> _dalLocalizer;
        public ServiceAdapter(
            AppDbContext context,
            IStringLocalizer<ServiceAdapter> dalLocalizer
,
            IRepositoryBase<Service, GetSpecality, AppDbContext> servRepo,
            IRepositoryBase<Service, GetService, AppDbContext> servAdm,
            IRepositoryBase<ServicePeriod, PeriodGetDto, AppDbContext> periodGServ,
            IRepositoryBase<ServiceCategory, GetCat, AppDbContext> catRepo) : base(context)
        {
            _dalLocalizer = dalLocalizer;
            _servRepo = servRepo;
            _servAdm = servAdm;
            _periodGServ = periodGServ;
            _catRepo = catRepo;
        }

        public virtual Task<IResult> AddComplaint(CreateComplaint complaint)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> AddPeriod(PeriodDto period)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> AddService(CreateSpecailty createSpecailty)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<List<GetSpecality>>> AllServices(int page, Guid? expertId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> CreateCategory(List<CreateCat> cats)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<string>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> DeleteCategory(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> DeleteComplaint(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> DeletePeriod(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<List<GetCat>>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<IEnumerable<GetComplaints>>> GetComplaints(Guid serviceId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<List<GetHeader>>> GetHeaders()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<List<GetService>>> GetServices(Guid? id, Guid? expertId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<List<PeriodGetDto>>> Periods(Guid specId, ReqFrom from)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<Detail>> ServiceDetail(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> Update(ServiceUpdateGet updateGet)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> UpdateCategory(CategoryUpdateGet updateGet)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<CategoryUpdateGet>> UpdateCategoryData(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> UpdatePeriod(ServicePeriodUpdateGet updateGet)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<ServiceUpdateGet>> UpdateServiceData(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<ServicePeriodUpdateGet>> UpdateServicePeriodData(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}