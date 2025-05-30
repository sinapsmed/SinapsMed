using AutoMapper;
using Entities.Concrete.Analyses;

namespace DataAccess.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Source --> Destination
            CreateMap<Entities.DTOs.AnalysisDtos.Analysis.Create, Analysis>();
            CreateMap<Entities.DTOs.AnalysisDtos.Category.CreateCategory, AnalysisCategory>();
        }
    }
}