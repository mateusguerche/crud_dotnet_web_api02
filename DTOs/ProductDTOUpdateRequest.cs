using System.ComponentModel.DataAnnotations;

namespace WebAPI_Projeto02.DTOs
{
    public class ProductDTOUpdateRequest
    {
        [Range(0, float.MaxValue, ErrorMessage = "The stock must be a non-negative value")]
        public float Stock { get; set; }
    }
}
