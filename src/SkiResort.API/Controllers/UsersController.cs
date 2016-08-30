using AdventureWorks.SkiResort.Infrastructure.Model;
using AdventureWorks.SkiResort.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace AdventureWorks.SkiResort.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UsersRepository _usersRepository;
        private readonly IConfigurationRoot _configuration;

        public UsersController(UsersRepository usersRepository, IConfigurationRoot configuration)
        {
            _usersRepository = usersRepository;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ApplicationUser> GetUserAsync()
        {
            return await _usersRepository.GetUserAsync(GetUser());
        }

        [HttpGet]
        [Route("photo/{userId}")]
        [ResponseCache(Duration = 180)]
        public async Task<IActionResult> GetPhotoAsync(string userId)
        {
            var photo = await _usersRepository.GetPhotoAsync(userId);
            if (photo == null)
            {
                return BadRequest();
            }
            return new FileStreamResult(new MemoryStream(photo), "image/jpeg");
        }
        string GetUser()
        {
            if (User.Identity.IsAuthenticated)
                return User.FindFirst("username").Value;
            else
                // Used in demos to allow not authenticated scenarios.
                return _configuration["DefaultUsername"];
        }
    }
}
