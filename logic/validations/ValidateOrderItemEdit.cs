using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Logic.dto.order_item;

namespace Logic.validations
{
    public class ValidateOrderItemEdit : AbstractValidator<OrderItemEditDto>
    {
        public ValidateOrderItemEdit() 
        {
            RuleFor(p => p.Quantity).GreaterThanOrEqualTo(0)
                .WithMessage("Quantity is mandatory and cannot be less than 0");
        }
    }
}
