using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.AccountantEntities;
using Entities.Concrete.Admin;
using Entities.Concrete.Analyses;
using Entities.Concrete.Clinics;
using Entities.Concrete.Experts;
using Entities.Concrete.Partners;
using Entities.Concrete.Staff;
using Entities.Concrete.UserEntities;
using Entities.DTOs.AuthDtos;
using Entities.DTOs.Helpers;

namespace DataAccess.Services.Abstract
{
    public interface IDAtaAccessService : IService
    {
        IResult CheckPasswordRequirements(string password);
        Token WorkSpaceToken(string email, Guid id);
        string GeneratePasswrod();
        Task<IDataResult<AppUser>> DecodeTokenAsync(string token);
        string HashPassword(string password, byte[] salt);
        string GetToken(IEnumerable<string> roles, AppUser user);
        string GetToken(IEnumerable<string> roles, Clinic user);
        string GetToken(IEnumerable<string> roles, Admin admin);
        string GetToken(IEnumerable<string> roles, Support admin);
        string GetToken(IEnumerable<string> roles, Accountant admin);
        string ExpertToken(Expert expert);
        Task<bool> SendVerificationSMSAsync(string phoneNumber, string code);
        double Distance(double lat1, double lon1, double lat2, double lon2);
        ExcelDataReturn<Analysis> ReadAnalysesFromExcel(string fileName, IQueryable<Partner> partners, IQueryable<AnalysisCategory> analysisCategories, IQueryable<Clinic> clinics);
        string AssetsPath();
        Task<string> ProblemsExcelData<T>(ExcelDataReturn<T> data)
             where T : class, new();
    }
}