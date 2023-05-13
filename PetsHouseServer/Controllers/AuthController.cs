using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsHouseServer.Models;

namespace PetsHouseServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private PetsHouseDatabaseContext _context;

        public AuthController(IConfiguration configuration, PetsHouseDatabaseContext context)
        {
            this._config = configuration;
            this._context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] UserAuth userCredentials)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Email == userCredentials.Email && x.Password == userCredentials.Password);
            if(user == null)
            {
                return BadRequest("Такого пользователя не существует!");
            } 
            return Ok("Авторизация прошла успешно!");
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] UserAuth userCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userCredentials.Email && x.Password == userCredentials.Password);
            if (user != null)
            {
                return BadRequest("Такой пользователь уже существует!");
            }
            _context.Users.Add(new Models.User
            {
                Email = userCredentials.Email,
                Password = userCredentials.Password,
            });
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        
    }

    public class UserAuth { 
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
