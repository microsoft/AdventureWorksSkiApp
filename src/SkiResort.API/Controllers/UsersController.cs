using AdventureWorks.SkiResort.Infrastructure.Model;
using AdventureWorks.SkiResort.Infrastructure.Repositories;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace AdventureWorks.SkiResort.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UsersRepository _usersRepository;

        public UsersController(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ApplicationUser> GetUserAsync()
        {
            return await _usersRepository.GetUserAsync(User.Identity.Name);
        }

        [HttpGet]
        [Route("photo/{userId}")]
        [ResponseCache(Duration = 180)]
        public async Task<IActionResult> GetPhotoAsync(string userId)
        {
            var photo = await _usersRepository.GetPhotoAsync(userId);
            if (photo == null)
            {
                return HttpBadRequest();
            }
            return new FileStreamResult(new MemoryStream(photo), "image/jpeg");
        }
    }
}
