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

        public int CategoryId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Name { get; set; }

        [Required]
        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName ="decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImageUrl { get; set; }

        public float Stock { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now.ToLocalTime();
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now.ToLocalTime();

        public Category? Category { get; set; }


    }
}
