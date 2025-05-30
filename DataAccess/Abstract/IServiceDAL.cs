using Core.Utilities.Results.Abstract;
using Entities.DTOs.Helpers;
using Entities.DTOs.ServiceDtos.Create;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.ServiceDtos.Update;
using Entities.DTOs.SpecalitiyDtos.Create;
using Entities.DTOs.SpecalitiyDtos.Get;

namespace DataAccess.Abstract
{
    public interface IServiceDAL
    {
        //Create Actions
        Task<IResult> AddComplaint(CreateComplaint complaint);
        Task<IResult> CreateCategory(List<CreateCat> cats);
        Task<IResult> AddService(CreateSpecailty createSpecailty);
        Task<IResult> AddPeriod(PeriodDto period);

        //Get Actions
        Task<IDataResult<IEnumerable<GetComplaints>>> GetComplaints(Guid serviceId);
        Task<IDataResult<ServicePeriodUpdateGet>> UpdateServicePeriodData(Guid id);
        Task<IDataResult<CategoryUpdateGet>> UpdateCategoryData(Guid id);
        Task<IDataResult<List<GetCat>>> GetCategories();
        Task<IDataResult<Detail>> ServiceDetail(Guid id);
        Task<IDataResult<List<GetService>>> GetServices(Guid? id, Guid? expertId);
        Task<IDataResult<List<GetHeader>>> GetHeaders();
        Task<IDataResult<List<GetSpecality>>> AllServices(int page, Guid? expertId);
        Task<IDataResult<List<PeriodGetDto>>> Periods(Guid specId, ReqFrom from);
        Task<IDataResult<ServiceUpdateGet>> UpdateServiceData(Guid id);

        //Update
        Task<IResult> UpdateCategory(CategoryUpdateGet updateGet);
        Task<IResult> Update(ServiceUpdateGet updateGet);
        Task<IResult> UpdatePeriod(ServicePeriodUpdateGet updateGet);

        //Delete Actions
        Task<IResult> DeleteComplaint(Guid id);
        Task<IResult> DeleteCategory(Guid id);
        Task<IDataResult<string>> Delete(Guid id);
        Task<IResult> DeletePeriod(Guid id);
    }

}