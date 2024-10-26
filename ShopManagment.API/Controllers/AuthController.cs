using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopManagment.Core.DTOs;
using ShopManagment.Core.Model;
using ShopManagment.Data.ContextClass;
using ShopManagment.Services;
using System.Security.Cryptography;
using System.Text;

namespace ShopManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly ApplicationDbContext _context;


        public AuthController(JwtTokenService jwtTokenService, ApplicationDbContext applicationDbContext)
        {
            _jwtTokenService = jwtTokenService;
            _context = applicationDbContext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO userDTO)
        {
            // Find the user in the database
            var user = _context.Users.FirstOrDefault(u => u.UserName == userDTO.Username);

            if (user == null)
            {
                return Unauthorized();
            }

            // Verify the password
            if (!VerifyPassword(userDTO.Password, user.PasswordHash))
            {
                return Unauthorized();
            }

            var token = _jwtTokenService.GenareteToken(user);

            return Ok(new { Token = token });

        }

        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] UserDTO userDTO)
        {
            string hashedPassword = HashPassword(userDTO.Password);

            var user = new User
            {
                UserName = userDTO.Username,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("Created Successfully");
        }

        // Helper function to hash passwords
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashBytes);
        }

        // Helper function to verify hashed password
        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            using var sha256 = SHA256.Create();
            // Compute the hash of the input password
            var inputPasswordHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));

            // Convert the byte array to a base64 string
            var inputPasswordHashBase64 = Convert.ToBase64String(inputPasswordHash);

            // Compare the computed hash with the stored hash
            return inputPasswordHashBase64 == storedPasswordHash;
        }

        


    }
}
