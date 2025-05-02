using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.Login
{
    public class LoginViewModel
    {
        [Display(Name = "Email", Prompt = "Enter email address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email must be provided")]
        public string Email { get; set; } = null!;

        [Display(Name = "Password", Prompt = "Enter password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Email must be provided")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}