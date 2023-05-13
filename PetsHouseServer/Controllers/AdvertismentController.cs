using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsHouseServer.Models;

namespace PetsHouseServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertismentController : ControllerBase
    {
        private IConfiguration _config;
        private PetsHouseDatabaseContext _context;
        public AdvertismentController(IConfiguration config, PetsHouseDatabaseContext context)
        {
            this._config = config;
            this._context = context;
        }

        [HttpPost("CreateAd")]
        public async Task<IActionResult> CreateAdvert([FromForm] AdvertismentCreateRequester advertisment)
        {

            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string fileUrl = baseUrl + "/images/empty.png";

            var petType = await _context.PetTypes.FirstOrDefaultAsync(x => x.TypeName.ToLower().Contains(advertisment.PetType.ToLower()));

            var adType = await _context.AdTypes.FirstOrDefaultAsync(x => x.AdTypeName.ToLower().Contains(advertisment.AdType.ToLower()));

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == advertisment.UserEmail);

            if (adType == null || petType == null || user == null)
            {
                return BadRequest(ModelState);
            }

            if (advertisment.Photo != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(advertisment.Photo.FileName);
                string extension = Path.GetExtension(advertisment.Photo.FileName);

                // Генерируем уникальное имя файла
                string uniqueFileName = $"{Guid.NewGuid().ToString()}{extension}";

                // Сохраняем файл в папку wwwroot/images
                string filePath = Path.Combine("wwwroot/images", uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await advertisment.Photo.CopyToAsync(fileStream);
                }

                fileUrl = $"{baseUrl}/images/{uniqueFileName}";
            }
            var newAdvert = new Advertisment
            {
                AdType = adType,
                PetType = petType,
                Description = advertisment.Description,
                User = user,
                Photo = fileUrl,
                Phone = advertisment.Phone,
                City = advertisment.CityName

            };
            await _context.Advertisments.AddAsync(newAdvert);
            await _context.SaveChangesAsync();
            return Ok(newAdvert);
        }
        [HttpPost("EditAdvert")]
        public async Task<IActionResult> EditAdvert([FromForm] AdvertismentEditRequester advertisment)
        {

            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string fileUrl = baseUrl + "/images/empty.png";

            var ad = await _context.Advertisments.FirstOrDefaultAsync(x => x.Id == advertisment.Id);

            
            if (ad == null)
            {
                return BadRequest(ModelState);
            }
            

            if (advertisment.Photo != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(advertisment.Photo.FileName);
                string extension = Path.GetExtension(advertisment.Photo.FileName);

                // Генерируем уникальное имя файла
                string uniqueFileName = $"{Guid.NewGuid().ToString()}{extension}";

                // Сохраняем файл в папку wwwroot/images
                string filePath = Path.Combine("wwwroot/images", uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await advertisment.Photo.CopyToAsync(fileStream);
                }

                fileUrl = $"{baseUrl}/images/{uniqueFileName}";
                ad.Photo = fileUrl;
            }
            if (advertisment.PetType != null)
            {
                var petType = await _context.PetTypes.FirstOrDefaultAsync(x => x.TypeName.ToLower().Contains(advertisment.PetType.ToLower()));
                if (petType != null)
                {
                    ad.PetType = petType;

                }
            }
            if (advertisment.AdType != null)
            {
                var adType = await _context.AdTypes.FirstOrDefaultAsync(x => x.AdTypeName.ToLower().Contains(advertisment.AdType.ToLower()));
                if (adType != null)
                {
                    ad.AdType = adType;
                }
            }
            
            if(advertisment.CityName != null && advertisment.CityName != String.Empty)
            {
                ad.City = advertisment.CityName;
            }
            if(advertisment.Description != null && advertisment.Description != String.Empty)
            {
                ad.Description = advertisment.Description;
            }
            if(advertisment.Phone != null && advertisment.Phone != String.Empty) 
            {
                ad.Phone = advertisment.Phone;
            }
            await _context.SaveChangesAsync();
            return Ok(ad);
        }
        [HttpGet("GetAdvert/{id}")]
        public async Task<IActionResult> GetAdvert(int id)
        {
            var ads = _context.Advertisments.AsQueryable();

            var adv = await ads.Where(x => x.Id == id)
                .Include(x=>x.PetType)
                .Include(x=>x.AdType)
                .Include(x=>x.User)
                .FirstOrDefaultAsync();
                                
            if (adv == null)
                return NotFound();
            return Ok(adv);
        }
        [HttpPost("GetAdverts")]
        public async Task<IActionResult> GetAdverts([FromForm] AdsFilter adsFilter)
        {
            var adsQuery = _context.Advertisments.AsQueryable();
            
            if(adsFilter.AdType != null && adsFilter.AdType != String.Empty)
            {
                adsQuery = adsQuery.Where(x => x.AdType.AdTypeName.ToLower().Contains(adsFilter.AdType.ToLower()));
            }
            if(adsFilter.PetType != null && adsFilter.PetType != String.Empty)
            {
                adsQuery = adsQuery.Where(x => x.PetType.TypeName.ToLower().Contains(adsFilter.PetType.ToLower()));
            }
            if (adsFilter.Search != null && adsFilter.Search != String.Empty)
            {
                adsQuery = adsQuery.Where(x => x.Description.ToLower().Contains(adsFilter.Search.ToLower()) || x.City.ToLower().Contains(adsFilter.Search.ToLower()));
            }
            var adsList = await adsQuery.ToListAsync();
            return Ok(adsList);
        }
        [HttpGet("DeleteAdvert/{id}")]
        public async Task<IActionResult> DeleteAdvert(int id)
        {
            var ads = _context.Advertisments.AsQueryable();

            var adv = await ads.Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (adv == null)
                return NotFound();

            _context.Advertisments.Remove(adv);
            await _context.SaveChangesAsync();
            return Ok("Удаление успешно");
        }
    }
    public class AdvertismentCreateRequester { 
        public string AdType { get; set; }
        public string Description { get; set; }
        public string PetType { get; set; }
        public IFormFile? Photo { get; set; }
        public string Phone { get; set; }
        public string CityName { get; set; }
        public string UserEmail { get; set; }
    }
    public class AdvertismentEditRequester
    {
        public int Id { get; set; }
        public string? AdType { get; set; }
        public string? Description { get; set; }
        public string? PetType { get; set; }
        public IFormFile? Photo { get; set; }
        public string? Phone { get; set; }
        public string? CityName { get; set; }
    }
    public class AdsFilter
    {
        
        public string? AdType { get; set; }
        public string? Search { get; set; }
        public string? PetType { get; set; }
    }
}
