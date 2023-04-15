using System.ComponentModel.DataAnnotations;

namespace PoCAPI.Models.DTO
{
    public class UserDTO
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
