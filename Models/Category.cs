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

        [Required]
        [StringLength(80)]
        public string? Name { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now.ToLocalTime();

        public DateTime UpdatedAt { get; set; } = DateTime.Now.ToLocalTime();

        public ICollection<Product>? Products { get; set; }
    }
}
