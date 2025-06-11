using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Logic.dto.authorization;
using Logic.dto.users;

namespace Logic.validations
{
    public class ValidateLoginDto : AbstractValidator<LoginDto>
    {
        public ValidateLoginDto() 
        {
            RuleFor(p => p.Username).NotEmpty()
                .WithMessage("Username is required and cannot be empty");

            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage("Password is required and cannot be empty");
        }
    }
}
