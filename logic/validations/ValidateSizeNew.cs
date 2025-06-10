using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Logic.dto.size;

namespace Logic.validations
{
    public class ValidateSizeNew : AbstractValidator<SizeNewDto>
    {
        public ValidateSizeNew()
        {
            RuleFor(p => p.Name).NotEmpty()
               .WithMessage("The size name cannot be an empty string and is required")
               .MaximumLength(10)
               .WithMessage("Size name cannot be bigger than 10 symbols");
        }
    }
}
