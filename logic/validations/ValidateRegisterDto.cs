using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Logic.dto.authorization;

namespace Logic.validations
{
    public class ValidateRegisterDto : AbstractValidator<RegisterDto>
    {
        public ValidateRegisterDto() 
        {
            RuleFor(p => p.Username).NotEmpty()
                .WithMessage("Username is required and cannot be empty")
                .MaximumLength(50)
                .WithMessage("Username lenght cannot be bigger than 50");

            RuleFor(p => p.Email).NotEmpty()
                .WithMessage("Email is required and cannot be empty")
                .EmailAddress()
                .WithMessage("Email is invalid. Please enter valid email")
                .MaximumLength(100)
                .WithMessage("Email lenght cannot be bigger than 100 symbols");

            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage("Password is required and cannot be empty")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long")
                .Must(p => p.Any(char.IsDigit))
                .WithMessage("Password must contain at least one digit")
                .Must(p => p.Any(char.IsLower))
                .WithMessage("Password must contain at least one lowercase letter")
                .Must(p => p.Any(char.IsUpper))
                .WithMessage("Password must contain at least one uppercase letter")
                .Must(p => p.Any(ch => !char.IsLetterOrDigit(ch)))
                .WithMessage("Password must contain at least one non-alphanumeric character")
                .Must(p => p.Distinct().Count() >= 1)
                .WithMessage("Password must contain at least one unique character");
        }
    }
}
