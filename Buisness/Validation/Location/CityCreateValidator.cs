using Buisness.Concrete;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.LocationDtos;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Buisness.Validation.Location
{
    public class CityCreateValidator : AbstractValidator<CreateCity>
    { 
        private readonly AppDbContext _context;
        public CityCreateValidator(AppDbContext context, IStringLocalizer<Validator> validaton)
        {
            _context = context; 

            RuleFor(c => c.Name)
                .Must(CheckCity).WithMessage(validaton["sameCityName"]); 
        }

        private bool CheckCity(string name)
        {
            return !_context.Cities.Any(c => c.Name.ToLower() == name.ToLower());
        }

    }
}