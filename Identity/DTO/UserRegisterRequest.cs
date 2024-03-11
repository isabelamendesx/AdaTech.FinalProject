using System.ComponentModel.DataAnnotations;

namespace Model.Application.API.DTO
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [EmailAddress(ErrorMessage = "Field {0} is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(50, ErrorMessage = "Fied {0} must have between {2} and {1} characters", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords must be equals")]
        public string ConfirmedPassword { get; set; }
    }
}
