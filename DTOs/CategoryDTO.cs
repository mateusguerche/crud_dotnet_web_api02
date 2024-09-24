using System.ComponentModel.DataAnnotations;
using WebAPI_Projeto02.Validation;

namespace WebAPI_Projeto02.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        [StringLength(80, ErrorMessage = "The name must be between 5 and 80 characters", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "The name can only contain letters and spaces")]
        [CategoriesValidation]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The Image URL is required")]
        public string? ImageUrl { get; set; }
    }
}
