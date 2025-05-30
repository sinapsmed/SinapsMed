using Buisness.Concrete;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.LocationDtos;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buisness.Validation.Location
{
    public class VillageCreateValidator : AbstractValidator<CreateVillage>
    {
        private readonly AppDbContext _context;

        public VillageCreateValidator(AppDbContext context, IStringLocalizer<Validator> validate)
        {
            _context = context;

            RuleFor(c => c.RegionId)
                .NotNull().WithMessage(validate["regionNull"])
                .NotEmpty().WithMessage(validate["regionNull"]);

            RuleFor(c => c.RegionId)
                .Must(CheckRegion).WithMessage(validate["regionNotFound"]);

            RuleFor(c => c.Name)
                .NotNull().WithMessage(validate["villageNameNull"])
                .NotEmpty().WithMessage(validate["villageNameNull"])
                .Must((model, name) => CheckVillage(model.RegionId, name)).WithMessage(validate["villageExists"]);
        }

        private bool CheckRegion(Guid id)
        {
            var region = _context.Regions.FirstOrDefault(c => c.Id == id);

            return region is not null;
        }
        private bool CheckVillage(Guid id, string name)
        {
            var region = _context.Regions.Include(c => c.Villages).FirstOrDefault(c => c.Id == id);
            if (region is null)
                return true;
            return !region.Villages.Any(c => c.Name.ToLower() == name.ToLower());
        }
    }
}