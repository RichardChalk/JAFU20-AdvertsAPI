using AdvertsAPI.User;
using System.IdentityModel.Tokens.Jwt;

namespace AdvertsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        // LOGIN ////////////////////////////////////////////////////////
        /// <summary>
        /// A chance to login
        /// </summary>
        /// <param 
        /// name="userLogin">Login with Username and password
        /// </param>
        /// <returns>
        /// User is logged in
        /// </returns>
        /// <remarks>
        /// Example end point: POST /api/Login
        /// </remarks>
        /// <response code="200">
        /// You were logged in successfully
        /// </response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200Created)]
        [ProducesDefaultResponseType]

        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User Not Found");
        }

        private UserModel Authenticate(UserLogin userLogin)
        {
            var currentUser = UserConstants.Users
                .FirstOrDefault(u =>
                u.UserName.ToLower() == userLogin.UserName.ToLower() &&
                u.Password == userLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }

        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.GivenName, user.GivenName),
                new Claim(ClaimTypes.Surname, user.SurName),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
