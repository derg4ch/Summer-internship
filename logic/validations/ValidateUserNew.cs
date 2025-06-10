using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Logic.dto.users;

namespace Logic.validations
{
    public class ValidateUserNew : AbstractValidator<UserNewDto>
    {
        public ValidateUserNew() 
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

            RuleFor(p => p.Password).NotEmpty()
                .WithMessage("Password is required and cannot be empty");
        }
    }
}
