using System;
using System.Collections.Generic;
using AdventureWorks.SkiResort.Infrastructure.Model;
using AdventureWorks.SkiResort.Infrastructure.Repositories;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using AdventureWorks.SkiResort.Infrastructure.Model.Enums;
using Newtonsoft.Json;

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
