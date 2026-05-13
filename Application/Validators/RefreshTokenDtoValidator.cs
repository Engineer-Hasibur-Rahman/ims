using FluentValidation;
using ims.Application.DTOs.Auth;

namespace ims.Application.Validators
{
    public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoValidator() 
        {
            RuleFor(x => x.RefreshToken).NotEmpty().MinimumLength(20);
        }
    }
}
