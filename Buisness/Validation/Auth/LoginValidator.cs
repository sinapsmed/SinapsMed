using Buisness.Concrete;
using Entities.DTOs.AuthDtos;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Buisness.Validation.Auth
{
    public class LoginValidator : AbstractValidator<Login>
    {
        private readonly IStringLocalizer<Validator> _localizer;
        public LoginValidator(IStringLocalizer<Validator> localizer)
        {
            _localizer = localizer;

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage(_localizer["nullmail"])
                .EmailAddress().WithMessage(_localizer["mail"]);
                
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8).WithMessage(_localizer["passwordMinLen"])
                .Matches(@"[A-Z]").WithMessage(_localizer["passwordUppercase"])
                .Matches(@"[a-z]").WithMessage(_localizer["passwordLowercase"])
                .Matches(@"\d").WithMessage(_localizer["passwordDigit"]);
        }
    }
}