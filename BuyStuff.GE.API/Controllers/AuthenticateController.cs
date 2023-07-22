using Azure;
using BuyStuff.GE.API.Infrastructure.Auth.JWT;
using BuyStuff.GE.Domain.Users;
using BuyStuff.GE.Domain.Users.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BuyStuff.GE.API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOptions<JWTConfiguration> _options;



        public AuthenticateController(
         UserManager<User> userManager,
         RoleManager<IdentityRole> roleManager,
         IOptions<JWTConfiguration> options)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _options = options;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = JWTHelper.GenerateSecurityToken(user.Email, user.Id, _options);

                return Ok(token);
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                throw new Exception("User Already Exists");
            User user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new Exception("There was an error creating user");

            var token = JWTHelper.GenerateSecurityToken(user.Email, user.Id, _options);

            return Ok(token);
        }
    }
}
