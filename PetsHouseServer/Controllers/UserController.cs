using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsHouseServer.Models;

namespace PetsHouseServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IConfiguration _configuration;
        PetsHouseDatabaseContext _context;
        public UserController(IConfiguration configuration, PetsHouseDatabaseContext context)
        {
            _configuration = configuration;
            _context = context; 
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("Удаление пользователя успешно");
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromForm] UserCreateRequester userCreate)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Email == userCreate.Email);
            if (user != null)
            {
                return BadRequest("Такой пользователь уже существует!");
            }
            var role =  await _context.Roles.Where(x=>x.Name.ToLower().Contains(userCreate.Role.ToLower())).FirstOrDefaultAsync();
            if(role ==  null)
            {
                return NotFound("Такой роли не существует!");
            }
            var userNew = new User
            {
                Email = userCreate.Email,
                FirstName = userCreate.FirstName,
                LastName = userCreate.LastName,
                Password = userCreate.Password,
                Role = role
            };
            _context.Users.Add(userNew);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers(string? searchRes)
        {
            var users = _context.Users.AsQueryable();
           
            if(searchRes != null)
            {
                string search = searchRes.ToLower();
                users = users.Where(
               x =>
               x.Email.ToLower().Contains(search) ||
               x.Password.ToLower().Contains(search) ||
               x.FirstName.ToLower().Contains(search) ||
               x.LastName.ToLower().Contains(search) ||
               x.Id.ToString().ToLower().Contains(search)
               );
            }
           
            var usersList = await users.ToListAsync();
            return Ok(usersList);
        }
        [HttpPost("EditUser")]
        public async Task<IActionResult> EditUser([FromForm] UserEditRequester userEditRequester)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Id == userEditRequester.Id);
            if(user == null) { return NotFound("Пользователь не найден!"); }

            
            if(userEditRequester.Password != null && userEditRequester.Password != String.Empty)
            {
                user.Password = userEditRequester.Password;
            }
            if (userEditRequester.FirstName != null && userEditRequester.FirstName != String.Empty)
            {
                user.FirstName = userEditRequester.FirstName;
            }
            if (userEditRequester.LastName != null && userEditRequester.LastName != String.Empty)
            {
                user.LastName = userEditRequester.LastName;
            }
            if (userEditRequester.Email != null && userEditRequester.Email != String.Empty)
            {
                user.Email = userEditRequester.Email;
            }
            if (userEditRequester.Role != null && userEditRequester.Role != String.Empty)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(x=>x.Name.ToLower() == userEditRequester.Role.ToLower());

                if(role == null) { return NotFound("Такой роли не существует"); }
               
                user.Role = role;
            }
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
    public class UserCreateRequester {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }
    public class UserEditRequester
    {
        public int Id { get; set; }
        public string? Email { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
    }
}
