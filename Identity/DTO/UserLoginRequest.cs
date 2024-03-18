using System.ComponentModel.DataAnnotations;

namespace Identity.DTO
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [EmailAddress(ErrorMessage = "Field {0} is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string Password { get; set; }
    }
}
