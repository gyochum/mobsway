using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mobsway.Web.Validators
{
    public class IsDateAfterValidationAttribute : ValidationAttribute, IClientValidatable
    {
        public string OtherDate { get; set; }
        public bool AllowEqualDate { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is DateTime))
            {
                return ValidationResult.Success;
            }

            var otherDate = validationContext.ObjectType.GetProperty(OtherDate);

            var otherDateValue = otherDate.GetValue(validationContext.ObjectInstance, null);

            if (otherDateValue != null && otherDateValue is DateTime)
            {
                bool valid = AllowEqualDate ? (DateTime)value >= (DateTime)otherDateValue : (DateTime)value > (DateTime)otherDateValue;

                if (!valid)
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "isdateaftervalidator"
            };

            rule.ValidationParameters["otherdate"] = OtherDate;
            rule.ValidationParameters["allowequaldate"] = AllowEqualDate;

            yield return rule;
        }
    }
}