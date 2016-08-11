using AdventureWorks.SkiResort.Infrastructure.Model;
using AdventureWorks.SkiResort.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdventureWorks.SkiResort.API.Controllers
{
    [Route("api/[controller]")]
    public class SummariesController : Controller
    {
        private readonly SummariesRepository _summariesRepository = null;

        
        public SummariesController(SummariesRepository summariesRepository)
        {
            _summariesRepository = summariesRepository;
        }

        [HttpGet()]
        public async Task<Summary> GetAsync()
        {
            return await _summariesRepository.GetAsync();
        }
    }
}
