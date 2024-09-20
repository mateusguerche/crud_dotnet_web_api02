using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPI_Projeto02.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "The name is required")]
        [StringLength(80, ErrorMessage = "The name must be between 5 and 80 characters", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "The name can only contain letters, numbers, and spaces")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The description is required")]
        [StringLength(300, ErrorMessage = "The description must be up to 300 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The price is required")]
        [Range(0.01, 9999999999.99, ErrorMessage = "The price must be a positive value")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The Image URL is required")]
        public string? ImageUrl { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "The stock must be a non-negative value")]
        public float Stock { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Category? Category { get; set; }


    }
}
