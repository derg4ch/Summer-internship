using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Logic.dto.authorization;

namespace Logic.validations
{
    public class ValidateRefresh : AbstractValidator<RefreshDto>
    {
        public ValidateRefresh() 
        {
            RuleFor(p => p.RefreshToken).NotEmpty()
                .WithMessage("Refresh token is required and cannot be empty");
        }
    }
}
