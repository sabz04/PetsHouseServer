using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsHouseServer.Models;

namespace PetsHouseServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        IConfiguration _configuration;
        PetsHouseDatabaseContext _context;
        public UtilitiesController(IConfiguration configuration, PetsHouseDatabaseContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }
        [HttpGet("GetPetTypes")]
        public async Task<IActionResult> GetPetTypes()
        {
            var petTypes = await _context.PetTypes.ToListAsync();

            return Ok(petTypes);
        }
        [HttpGet("GetAdTypes")]
        public async Task<IActionResult> GetAdTypes()
        {
            var adTypes = await _context.AdTypes.ToListAsync();
            return Ok(adTypes);
        }
    }
}
