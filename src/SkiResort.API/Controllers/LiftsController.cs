using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.SkiResort.Infrastructure.Helpers;
using AdventureWorks.SkiResort.Infrastructure.Model;
using AdventureWorks.SkiResort.Infrastructure.Model.Enums;
using AdventureWorks.SkiResort.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.SkiResort.Infrastructure.CosmosDB.Repositories;
using SkiResort.Infrastructure.CosmosDB.Model;

namespace AdventureWorks.SkiResort.API.Controllers
{
    [Route("api/[controller]")]
    public class LiftsController : Controller
    {
        private readonly LiftsRepository _liftsRepository = null;
        private readonly LiftLinesRepository _liftLinesRepository = null;

        public LiftsController(LiftsRepository liftsRepository, LiftLinesRepository liftLinesRepository)
        {
            _liftsRepository = liftsRepository;
            _liftLinesRepository = liftLinesRepository;
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
            var lifts = await _liftsRepository.GetNearByAsync(latitude, longitude);
            var liftCounts = await _liftLinesRepository.GetLiftSkiersWaitingAsync();
            var liftHistory = await _liftLinesRepository.GetLiftWaitHistoryWaitingAsync(TimeSpan.FromMinutes(30));

            if (liftCounts != null && liftHistory != null)
            {
                foreach (var lift in lifts)
                {
                    lift.WaitingTime = GetWaitingTime(liftCounts, lift);

                    var history = liftHistory.Where(lh => lh.Rkey == lift.Name)
                                             .Select(lh => Tuple.Create(lh.Time, lh.Skiercount))
                                             .ToList();

                    lift.StayAway = ShowStayAway(lift) ? true : await AnomalyDetector.SlowChairliftAsync(history);
                }
            }

            return lifts;
        }

        private int GetWaitingTime(List<LiftLines> liftCounts, Lift lift)
        {
            if (lift.Name == "Education Hill")
                return 12; // for the demo porpouse this value is fixed.

            return lift.Status != LiftStatus.Open ? -1 :
                ComputeWaitTime(liftCounts.FirstOrDefault(l => l.Name == lift.Name)?.SkierCount);
        }

        private int ComputeWaitTime(int? skiersWaiting)
        {
            if (!skiersWaiting.HasValue)
            {
                return -1;
            }

            // 40 passengers/minute on a modern quad
            // A more realistic setup would be to include lift speed as part of the lifts table
            return skiersWaiting.Value / 40;
        }

        bool ShowStayAway(Lift lift)
        {
            // Show Stay Away icon, only to use in a demo to show how the app shows it.
            return lift.LiftId == 1;
        }
    }
}
