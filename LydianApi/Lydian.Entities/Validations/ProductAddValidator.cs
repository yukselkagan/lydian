using FluentValidation;
using Lydian.Entities.Dto;
using Lydian.Entities.Validations.CustomValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Entities.Validations
{
    public class ProductAddValidator : AbstractValidator<ProductAddDto>
    {
        public ProductAddValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().MinimumLength(3).MaximumLength(40);          
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0.1).LessThanOrEqualTo(1000);
            RuleFor(x => x.Description).NotNull();
            RuleFor(x => x.ImagePath).Length(3, 100);

            RuleFor(x => x.ProductName).Must(NotForbiddenName).WithMessage("Used forbidden product name");
            RuleFor(x => x.Description).HaveNotForbiddenWords();
            RuleFor(x => x.Description).CustomDescriptionControl();

        }

        public bool NotForbiddenName(string productName)
        {
            if (productName == "Forbidden Product Name")
            {
                return false;
            }
            return true;
        }

    }
}
