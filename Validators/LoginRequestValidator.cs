using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Validators
{
    public class LoginRequestValidator:AbstractValidator<Models.DTO.LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();

        }
    }
}
