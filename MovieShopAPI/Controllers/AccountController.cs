using ApplicationCore.Models;
using ApplicationCore.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        // injecting for secretKey
        private readonly IConfiguration _configuration;
        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
            // expecting the info from body of the request
        {
            var user = await _accountService.CreateUser(model);
            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var user = await _accountService.ValidateUser(model);
            if(user != null)
            {
                // create token
                var jwtToken = CreateJwt(user);
                return Ok(new { token = jwtToken });
            }

            // IOS, Android, Angular, React, Java will use this
            // We will create token (instead of cookie), called JWT (JSON Web Token) jwt.io
            // industry standard e.g. ASP creates Token and Java can decode
            // Client sends email/password to API (POST) --> API will validate the info and create JWT and send to client
            // Client(IOS, Android) will store/decode JWT in device
            // Angular, React can store in localstorage or sessionstorage

            // when client needs secured information or needs to perform any operation that requires authentiation
            // client needs to send JWT to the API in the HTTP Header
            // JWT will be validated by API and send the data back to the client
            // if JWT is invalid or expired, then API will send 401 Unauthorized error

            // if null
            throw new UnauthorizedAccessException("There is no matching Email/Password. Please check email and password again");
            return Unauthorized(new { errorMessage = "Please check email and password" });

        }

        private string CreateJwt(UserInfoResponseModel user)
        {
            // need to install IdentityModel.Tokens and .Tokens.Jwt
            // create Claims object

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.FamilyName,user.LastName),
                new Claim(JwtRegisteredClaimNames.GivenName,user.FirstName),
                new Claim("Country","USA"),
                new Claim("language","english")
            };

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            // first need to specify a secret key
            // read key from appsetting
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["secretKey"]));

            // next need to specify the algorithm
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // then need to specify the expiration of jwt
            var tokenExpiration = DateTime.UtcNow.AddHours(2);

            // finally need to create object with all the above info to create jwt
            var tokenDetails = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Expires = tokenExpiration,
                SigningCredentials = credentials,
                Issuer = "MovieShop Sean",
                Audience = "MovieShop Clients"
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var encodedJwt = tokenHandler.CreateToken(tokenDetails);
            return tokenHandler.WriteToken(encodedJwt);
        }
    }
}
