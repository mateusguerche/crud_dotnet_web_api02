using System.ComponentModel.DataAnnotations;

namespace WebAPI_Projeto02.Validation
{
    public class CategoriesValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, 
            ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) 
                return ValidationResult.Success;

            
            var firstLetter = value.ToString()[0].ToString();
            if (firstLetter != firstLetter.ToUpper())
            
                return new ValidationResult("The first letter of the product name must be uppercase.");
            

            return ValidationResult.Success;
        }
    }
}
