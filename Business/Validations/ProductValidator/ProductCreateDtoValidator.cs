using Entities.DTOs.ProductDTOs;
using FluentValidation;

namespace ECommerce.Business.Validations.ProductValidator
{
    public class ProductCreateDTOValidator : AbstractValidator<ProductCreateDTO>
    {
        public ProductCreateDTOValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(x => x.Discount).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).WithMessage("Discount must be between 0 and 100");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category ID must be greater than 0");
            RuleFor(x => x.PhotoUrl).NotEmpty().WithMessage("Photo URL is required");
            RuleFor(x => x.CreatedDate).NotEmpty().WithMessage("Created date is required");
            RuleFor(x => x.SpecificationAddDTOs).NotEmpty().WithMessage("Specifications are required");
            // RuleForEach(x => x.SpecificationAddDTOs).SetValidator(new SpecificationAddDTOValidator());
        }
    }
}
