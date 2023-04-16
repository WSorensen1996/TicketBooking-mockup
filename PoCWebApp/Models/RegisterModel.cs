using System.ComponentModel.DataAnnotations;

namespace PoCWebApp.Models
{
    public class RegisterModel
    {


        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

    }
}
