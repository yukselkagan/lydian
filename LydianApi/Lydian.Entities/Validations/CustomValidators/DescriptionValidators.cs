using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Entities.Validations.CustomValidators
{
    public static class DescriptionValidators
    {
        public static IRuleBuilder<T, string> HaveNotForbiddenWords<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var control = ruleBuilder.Must(description => !description.Contains("forbidden"))
                .WithMessage("Used forbidden words in {PropertyName}");
            return control;
        }

        public static IRuleBuilder<T, string> CustomDescriptionControl<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var control = ruleBuilder.Must((objectRoot, description, context) =>
            {
                context.MessageFormatter.AppendArgument("CustomArgument", "something created random");

                if(description.Contains("custom validation error"))
                {
                    return false;
                }
                return true;
            }).WithMessage("Custom validation error information : {CustomArgument}");

            return control;
        }

    }
}
