using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI_Projeto02.Models
{
    [Table("categories")]
    public class Category
    {
        public Category() 
        { 
            Products = new Collection<Product>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        [StringLength(80, ErrorMessage = "The name must be between 5 and 80 characters", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "The name can only contain letters and spaces")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The Image URL is required")]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<Product>? Products { get; set; }
    }
}
