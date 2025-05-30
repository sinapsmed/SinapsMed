using System.Text.RegularExpressions;
using Buisness.Concrete;
using Entities.DTOs.AuthDtos;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Buisness.Validation.Auth
{
    public class RegisterValidator : AbstractValidator<Register>
    {
        private readonly IStringLocalizer<Validator> _loclizer;
        public RegisterValidator(IStringLocalizer<Validator> loclizer)
        {
            _loclizer = loclizer;
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(_loclizer["name"]);
            RuleFor(c=>c.SurName)
                .NotEmpty().WithMessage(_loclizer["surname"]);
            RuleFor(c=>c.Mail)
                .NotEmpty().WithMessage(_loclizer["nullmail"])
                .EmailAddress().WithMessage(_loclizer["mail"]);
            RuleFor(c=>c.PhoneNumber)
                .NotEmpty().WithMessage(_loclizer["nullphone"])
                .Must(BeAValidPhoneNumber).WithMessage(_loclizer["phone"]);
            RuleFor(c=>c.Gender)
                .NotEmpty().WithMessage(_loclizer["gender"]);
        }
        private bool BeAValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber != null && Regex.IsMatch(phoneNumber, @"^\+?[1-9]\d{1,14}$");
        }
    }
}