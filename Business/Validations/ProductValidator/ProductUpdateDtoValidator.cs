using Entities.DTOs.ProductDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Validations.ProductValidator
{
    public class ProductUpdateDTOValidator : AbstractValidator<ProductUpdateDTO>
    {
        public ProductUpdateDTOValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name cannot be empty");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(x => x.Discount).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).WithMessage("Discount must be between 0 and 100");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
            RuleFor(x => x.IsFeatured).NotNull().WithMessage("IsFeatured cannot be null");
            RuleFor(x => x.Status).NotNull().WithMessage("Status cannot be null");
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId must be greater than 0");
            RuleFor(x => x.PhotoUrl).NotEmpty().WithMessage("Photo URL cannot be empty");
        }
    }
}
