using Auth_DoDoApp_API.Common;
using Auth_DoDoApp_API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDo_Auth_DAL.Models;

namespace Auth_DoDoApp_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToDoAuthController : ControllerBase
    {
        private readonly UserManager<AuthToDoUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AuthToDoUserEntity> _signInManager;

        public ToDoAuthController(UserManager<AuthToDoUserEntity> userManager, SignInManager<AuthToDoUserEntity> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM registerVM)
        {
            if (registerVM == null)
            {
                return BadRequest("RegisterVM is null");
            }

            var existingUser = await _userManager.FindByNameAsync(registerVM.Name);
            if (existingUser != null)
            {
                return Conflict(new Response { Status = "Error", Message = "User already exists!" });
            }

            var appUser = new AuthToDoUserEntity
            {
                UserName = registerVM.Name,
                AccountType = registerVM.AccountType,
                Email = registerVM.Email,
                PhoneNumber = registerVM.PhoneNo,
                Password = registerVM.Password,
                ShopName = registerVM.ShopName,
                BusinessType = registerVM.BusinessType,
                UserRole = registerVM.UserRole,
                IsDeleted = registerVM.IsDeleted
            };

            var result = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            if (!await _roleManager.RoleExistsAsync(registerVM.UserRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(registerVM.UserRole));
            }

            if (await _roleManager.RoleExistsAsync(registerVM.UserRole))
            {
                await _userManager.AddToRoleAsync(appUser, registerVM.UserRole);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {
            if (loginVM == null)
            {
                return BadRequest("LoginVM is null");
            }

            var user = await _userManager.FindByNameAsync(loginVM.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginVM.Password))
            {
                return Unauthorized();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JsonWebTokenKeys:IssuerSigningKey"]));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                user = user,
                Role = userRoles,
                status = "User Login Successfully"
            });
        }
    }
}
