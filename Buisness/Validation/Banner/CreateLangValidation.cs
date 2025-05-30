using Buisness.Concrete;
using Entities.DTOs.BannerDtos;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Buisness.Validation.Auth
{ 
    public class CreateLangValidator : AbstractValidator<LangCreate>
    {
        private readonly IStringLocalizer<Validator> _localizer;
        public CreateLangValidator(IStringLocalizer<Validator> localizer)
        {
            _localizer = localizer;
 
            RuleFor(c => c.Description)
                .NotEmpty().WithMessage(_localizer["bannerlangDesc"]);

            RuleFor(c => c.Title)
                .NotEmpty().WithMessage(_localizer["bannerlangTitle"]);
        }
    }
}