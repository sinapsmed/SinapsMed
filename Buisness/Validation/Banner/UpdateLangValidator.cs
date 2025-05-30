using Buisness.Concrete;
using Entities.DTOs.BannerDtos;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Buisness.Validation.Auth
{ 
    public class UpdateLangValidator : AbstractValidator<LangUpdate>
    {
        private readonly IStringLocalizer<Validator> _localizer;
        public UpdateLangValidator(IStringLocalizer<Validator> localizer)
        {
            _localizer = localizer;

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage(_localizer["bannerlangDesc"]);

            RuleFor(c => c.Title)
                .NotEmpty().WithMessage(_localizer["bannerlangTitle"]);
        }

        public bool AccecptedLanguages(string language)
        {
            List<string> languages = new List<string> { "az", "tr" };
            return languages.Contains(language);
        }
    }
}