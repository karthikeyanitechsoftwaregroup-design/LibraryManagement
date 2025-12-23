using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Validation
{
    public class CurrentYearAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            int year = (int)value;
            int currentYear = DateTime.Now.Year;

            if (year > currentYear)
            {
                return new ValidationResult("Current year must be up to this year");
            }

            return ValidationResult.Success;
        }
    }
}
