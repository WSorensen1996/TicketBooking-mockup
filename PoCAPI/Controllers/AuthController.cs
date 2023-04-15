


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoCAPI.Models.DTO;

namespace PoCAPI.Controllers
{

    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<string> AuthenticateUserAsync([FromBody] UserDTO _user)
        {
            // Check if the email and password are valid
            var user = await _userManager.FindByEmailAsync(_user.Email);
            if (user == null)
            {
                // Return null if the user does not exist
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, _user.Password, false);
            if (!result.Succeeded)
            {
                // Return null if the password is incorrect
                return null;
            }



            // Generate a new token
            var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "AccessToken");

            return token;
        }


    }

}