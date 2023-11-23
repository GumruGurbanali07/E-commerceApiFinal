using Entities.DTOs.OrderDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Validations.OrderValidator
{
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDTO>
    {
        public OrderCreateDtoValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("ProductId must be greater than 0");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(x => x.DeliveryAddress).NotEmpty().WithMessage("Delivery address cannot be empty");
        }
    }
}
