using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Contracts.Repository;
using ApplicationCore.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        public AccountController(IAccountService accountService, IConfiguration configuration, IUserRepository userRepository)
        {
            _accountService = accountService;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            var user = await _accountService.CreateUser(model);
            return Ok(user);
        }

        [HttpGet]
        [Route("check-email")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var emailExist = await _userRepository.GetUserByEmail(email);
            if(emailExist == null)
            {
                return Ok(false);
            }
            return Ok(true);
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var user = await _accountService.ValidateUser(model);
            if (user != null)
            {
                var jwtToken = CreateJwtToken(user);
                return Ok(new {token = jwtToken});
            }

            //throw new UnauthorizedAccessException("Please check email and password");

            return Unauthorized(new { errorMessage = "Please check email and password" });

        }

        private string CreateJwtToken(UserInfoResponseModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim("Country","USA"),
                new Claim("Language","English")
            };
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            // Specify a secret key
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["secretKey"]));

            // Specify the algorithm
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // Specify the expiration of the token
            var tokenExpiration = DateTime.UtcNow.AddHours(2);

            // Create and object with all the above information so create the token
            var tokenDetails = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Expires = tokenExpiration,
                SigningCredentials = credentials,
                Issuer = "MovieShop, Inc",
                Audience = "MovieShop Clients"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var encodedJwt = tokenHandler.CreateToken(tokenDetails);
            return tokenHandler.WriteToken(encodedJwt); 

        }

    }
}
