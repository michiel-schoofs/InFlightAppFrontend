using System.ComponentModel.DataAnnotations;

namespace InFlightAppBACKEND.Models.DTO
{
    public class LoginDTO{
        [Required(AllowEmptyStrings =false,ErrorMessage ="You need to login via a username")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings =false,ErrorMessage = "Please provide a password")]
        public string Password { get; set; }
    }
}
