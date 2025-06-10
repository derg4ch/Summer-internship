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

            RuleFor(p => p.Status).NotEmpty().
                WithMessage("Status is required.")
               .Must(status => new[] { "pending", "completed", "shipped" }.Contains(status.ToLower()))
               .WithMessage("Status must be one of the following: Pending, Completed, Shipped.")
               .MaximumLength(20)
               .WithMessage("Status cannot be bigger than 20 symbols");

            RuleFor(p => p.ClothingItemId).GreaterThan(0)
                .WithMessage("ClothingItemId is required and cannot be empty");

            RuleFor(p => p.ClothingItemId).GreaterThanOrEqualTo(0)
                .WithMessage("The quantity is mandatory and cannot be less than 0");
        }
    }
}
