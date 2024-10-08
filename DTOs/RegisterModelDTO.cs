using System.ComponentModel.DataAnnotations;

namespace WebAPI_Projeto02.DTOs
{
    public class RegisterModelDTO
    {
        [Required(ErrorMessage = "User name is required")]
        public string? UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email {  get; set; } 

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
