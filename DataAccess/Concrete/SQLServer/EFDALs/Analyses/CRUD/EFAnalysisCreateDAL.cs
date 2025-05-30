using System.Threading.Tasks;
using Core.DataAccess;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.SuccessResult;
using Core.Utilities.Static;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Services.Abstract;
using Entities.Concrete.Analyses;
using Entities.Concrete.Clinics;
using Entities.Concrete.Partners;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AnalysisDtos.Category;
using Entities.DTOs.Helpers;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
namespace DataAccess.Concrete.SQLServer.EFDALs.Analyses.CRUD
{
    public class EFAnalysisCreateDAL : AnalysesAdapter
    {
        private readonly IWebHostEnvironment _host;
        private readonly IEmailService _email;
        private readonly IDAtaAccessService _dataService;

        public EFAnalysisCreateDAL(
            AppDbContext context,
            IStringLocalizer<AnalysesAdapter> dalLocalizer,
            IRepositoryBase<Analysis, Get, AppDbContext> repo,
            IDAtaAccessService dataService
,
            IRepositoryBase<AnalysisCategory, GetCats, AppDbContext> repoCat,
            IWebHostEnvironment host,
            IEmailService email) : base(context, dalLocalizer, repo, repoCat)
        {
            _dataService = dataService;
            _host = host;
            _email = email;
        }

        public override async Task<IResult> Add(Create create)
        {
            Analysis analysis = create.Map<Analysis, Create>();

            using (var context = new AppDbContext())
            {
                return await _repo.AddAsync(analysis, context);
            }
        }
        

        public override async Task<IResult> AddCat(CreateCategory createCategory)
        {
            AnalysisCategory category = createCategory.Map<AnalysisCategory, CreateCategory>();


            using (var context = new AppDbContext())
            {
                return await _repoCat.AddAsync(category, context);
            }
        }

        public override async Task<IResult> AddList(Microsoft.AspNetCore.Http.IFormFile file, string agentMail)
        {
            using (var context = new AppDbContext())
            {
                var partners = context.Set<Partner>();
                var categories = context.Set<AnalysisCategory>();

                string folder = Path.Combine(_host.WebRootPath, "assets", "Excels");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Path.Combine(folder, file.FileName);

                using (FileStream fileStream = File.Create(fileName))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }

                BackgroundJob.Enqueue(() => ProcessExcelFile(fileName, agentMail));

                return new SuccessResult(_dalLocalizer["jobStarted"]);
            }
        }
        public async Task ProcessExcelFile(string fileName, string agentMail)
        {
            using (var context = new AppDbContext())
            {
                var partners = context.Set<Partner>();
                var categories = context.Set<AnalysisCategory>();
                var clinics = context.Set<Clinic>();

                ExcelDataReturn<Analysis> result = _dataService.ReadAnalysesFromExcel(fileName, partners, categories, clinics);
                context.Analyses.AddRange(result.Datas);
                context.SaveChanges();

                var filePath = await _dataService.ProblemsExcelData<Analysis>(result);

                await _email.SendEmailAsync(agentMail,
                 "The analysis download process has finished.",
                "<p>The analysis download process has completed. For more information, see the document below. These are the problems the system encountered during the download.</p>");

                File.Delete(filePath);
                File.Delete(fileName);
            }
        }
    }
}