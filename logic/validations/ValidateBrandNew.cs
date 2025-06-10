using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Logic.dto.brand;

namespace Logic.validations
{
    public class ValidateBrandNew : AbstractValidator<BrandNewDto>
    {
        public ValidateBrandNew() 
        {
            RuleFor(p => p.Name).NotEmpty()
                .WithMessage("The brand name is mandatory and cannot be empty")
                .MaximumLength(50)
                .WithMessage("Brand name cannot be bigger than 50 symbols");

            RuleFor(p => p.Country).NotEmpty()
                .WithMessage("The countre of brand is mandatory and cannot be empty")
                .MaximumLength(50)
                .WithMessage("Country name cannot be bigger than 50 symbols");
        }
    }
}
