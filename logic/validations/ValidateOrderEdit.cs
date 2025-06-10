using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Logic.dto.order;

namespace Logic.validations
{
    public class ValidateOrderEdit : AbstractValidator<OrderEditDto>
    {
        public ValidateOrderEdit() 
        {
            RuleFor(p => p.Status).NotEmpty()
               .WithMessage("Status is required.")
               .Must(status => new[] { "pending", "completed", "shipped" }.Contains(status.ToLower()))
               .WithMessage("Status must be one of the following: Pending, Completed, Shipped.")
               .MaximumLength(20)
               .WithMessage("Status cannot be bigger than 20 symbols");
        }
    }
}
