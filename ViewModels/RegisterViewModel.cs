using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GemStore.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password did not match.")]
        public string ConfirmPassword { get; set; }
    }
}
