using AdventureWorks.SkiResort.Infrastructure.Model;
using AdventureWorks.SkiResort.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorks.SkiResort.API.Controllers
{
    [Route("api/[controller]")]
    public class LiftsController : Controller
    {
        private readonly LiftsRepository _liftsRepository = null;

        
        public LiftsController(LiftsRepository liftsRepository)
        {
            _liftsRepository = liftsRepository;
        }

        [HttpGet("{id}")]
        public async Task<Lift> GetAsync(int id)
        {
            return await _liftsRepository.GetAsync(id);
        }

        [HttpGet]
        [Route("nearby")]
        public async Task<IEnumerable<Lift>> GetNearByAsync(double latitude, double longitude)
        {
            return await _liftsRepository.GetNearByAsync(latitude, longitude);
        }
    }
}
