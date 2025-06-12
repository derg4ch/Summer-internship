using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Logic.dto.order;

namespace Logic.validations
{
    public class ValidateOrderNew : AbstractValidator<OrderNewDto>
    {
        public ValidateOrderNew()
        {
            RuleFor(p => p.UserId).GreaterThan(0)
                .WithMessage("UserId is required and cannot be empty");

            RuleFor(p => p.ClothingItemId).GreaterThan(0)
                .WithMessage("ClothingItemId is required and cannot be empty");

            RuleFor(p => p.ClothingItemId).GreaterThanOrEqualTo(0)
                .WithMessage("The quantity is mandatory and cannot be less than 0");
        }
    }
}
