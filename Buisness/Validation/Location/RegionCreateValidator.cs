using Buisness.Concrete;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.LocationDtos;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buisness.Validation.Location
{
    public class RegionCreateValidator : AbstractValidator<CreateRegion>
    {
        private readonly AppDbContext _context;
        public RegionCreateValidator(AppDbContext context, IStringLocalizer<Validator> validate)
        {
            _context = context;

            RuleFor(c => c.CityId)
                .NotEmpty().WithMessage(validate["CityIdNull"])
                .NotNull().WithMessage(validate["CityIdNull"]);

            RuleFor(c => c.CityId)
                .Must(CheckCity).WithMessage(validate["cityNotFound"]);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(validate["regionNameNull"])
                .Must((model, name) => CheckRegion(model.CityId, name))
                .WithMessage(validate["regionAlreadyAdded"]);
        }
        private bool CheckCity(Guid id)
        {
            var city = _context.Cities.FirstOrDefault(c => c.Id == id);

            return city is not null;
        }
        private bool CheckRegion(Guid id, string name)
        {
            var city = _context.Cities
                .Include(c => c.Regions)
                .FirstOrDefault(c => c.Id == id); 
            return !city.Regions.Any(c => c.Name.ToLower() == name.ToLower());
        }
    }
}