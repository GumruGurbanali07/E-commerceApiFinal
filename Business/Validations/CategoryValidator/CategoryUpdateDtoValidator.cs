using Entities.DTOs.CategoryDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Validations.CategoryValidator
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDTO>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category name cannot be empty");
            RuleFor(x => x.PhotoUrl).NotEmpty().WithMessage("Photo URL cannot be empty");
        }
    }
}
