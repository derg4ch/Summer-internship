using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Logic.dto.order_item;

namespace Logic.validations
{
    public class ValidateOrderItemNew : AbstractValidator<OrderItemNewDto>
    {
        public ValidateOrderItemNew()
        {
            RuleFor(p => p.OrderId).GreaterThan(0)
                .WithMessage("OrderId is required and cannot be less than 1");

            RuleFor(p => p.ClothingItemId).GreaterThan(0)
                .WithMessage("ClothingItemId is required and cannot be less than 1");

            RuleFor(p => p.Quantity).GreaterThanOrEqualTo(0)
               .WithMessage("Quantity is required and cannot be less than 0");
        }
    }
}
