using System.ComponentModel.DataAnnotations;

namespace BookTheRoom.WebUI.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email Address / Username")]
        [Required(ErrorMessage = "Email address or username is required.")]
        public string EmailOrUsername { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
