using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.CustomValidators
{
    public class MinimumDateValidatorAttribute : ValidationAttribute
    {
        private readonly int _minimumYear = 2000;
        private readonly string _errorMessage = "Year should not be less than {0}";

        public MinimumDateValidatorAttribute()
        {
            
        }
        public MinimumDateValidatorAttribute(int MinimumYear)
        {
            _minimumYear = MinimumYear;
        }

        protected override ValidationResult? IsValid(object? value,
            ValidationContext validationContext)
        {
           if (value != null)
            {
                DateTime date = (DateTime)value;

                if (date.Year < _minimumYear)
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? _errorMessage, _minimumYear));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            return null;
        }
    }
}
