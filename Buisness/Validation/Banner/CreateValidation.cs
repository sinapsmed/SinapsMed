using Buisness.Concrete;
using Entities.DTOs.BannerDtos;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Buisness.Validation.Auth
{
    public class CreateValidator : AbstractValidator<Create>
    {
        public CreateValidator(IStringLocalizer<Validator> localizer)
        {
            RuleFor(c => c.ImageUrl)
                .NotEmpty().WithMessage(localizer["bannerImageUrl"]);

            RuleFor(c => c.Link)
                .NotEmpty().WithMessage(localizer["bannerLink"]);

            RuleForEach(c => c.LanguagesCreate)
                .SetValidator(new CreateLangValidator(localizer));
        }
    }
}