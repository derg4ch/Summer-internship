using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Logic.dto.clothing_item;

namespace Logic.validations
{
    public class ValidateClothingItemEdit : AbstractValidator<ClothingItemEditDto>
    {
        public ValidateClothingItemEdit()
        {
            RuleFor(p => p.Name).NotEmpty()
                .WithMessage("The name of the thing cannot be empty and is required")
                .MaximumLength(100)
                .WithMessage("Name of clothing item cannot be bigger than 100 symbols");

            RuleFor(p => p.SizeId).GreaterThan(0)
                .WithMessage("SizeID cannot be less than or equal to 0");

            RuleFor(p => p.BrandId).GreaterThan(0)
                .WithMessage("BrandID cannot be less than or equal to 0");

            RuleFor(p => p.Price).GreaterThan(0)
                .WithMessage("The price of the goods cannot be less than or equal to 0");
            
            RuleFor(p => p.Quantity).GreaterThanOrEqualTo(0)
                .WithMessage("The quantity of the goods cannot be less than 0");
        }
    }
}
