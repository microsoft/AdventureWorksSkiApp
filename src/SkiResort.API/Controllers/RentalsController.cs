using AdventureWorks.SkiResort.Infrastructure.Model;
using AdventureWorks.SkiResort.Infrastructure.Repositories;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorks.SkiResort.API.Controllers
{
    [Route("api/[controller]")]
    public class RentalsController : Controller
    {
        private readonly RentalsRepository _rentalsRepository = null;

        public RentalsController(RentalsRepository rentalsRepository)
        {
            _rentalsRepository = rentalsRepository;
        }

        [HttpGet("{id}")]
        public async Task<Rental> GetAsync(int id)
        {
            return await _rentalsRepository.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _rentalsRepository.GetAllAsync();
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody]Rental rental)
        {
            if (ModelState.IsValid)
            {
                await _rentalsRepository.AddAsync(rental);
                return Ok();
            }
            else
            {
                return HttpBadRequest(ModelState);
            }
        }

        [HttpPut]
        public async Task UpdateAsync([FromBody]Rental rental)
        {
            await _rentalsRepository.UpdateAsync(rental);
        }
        
        [HttpDelete]
        public async Task DeleteAsync(int id)
        {
            await _rentalsRepository.DeleteAsync(id);
        }

        [HttpGet]
        [Route("check_high_demand")]
        public bool CheckHighDemand(DateTimeOffset date)
        {
            return false;
        }
    }
}
