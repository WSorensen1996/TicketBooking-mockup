using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoCWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {


        [PersonalData]
        [Column(TypeName = "nvarchar(100")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100")]
        public string LastName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100")]
        public string City { get; set; }



    }
}