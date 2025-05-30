using Core.Entities;
using Entities.Concrete.Clinics;
using Entities.Concrete.Partners;

namespace Entities.Concrete.Analyses
{
    public class Analysis : IEntity
    {
        public Guid Id { get; set; }
        public Partner Partner { get; set; }
        public Guid PartnerId { get; set; }
        public AnalysisCategory? Category { get; set; }
        public Guid CategoryId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public Clinic Clinic { get; set; }
        public Guid ClinicId { get; set; }
    }
}