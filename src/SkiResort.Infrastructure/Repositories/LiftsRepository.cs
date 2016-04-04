using AdventureWorks.SkiResort.Infrastructure.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventureWorks.SkiResort.Infrastructure.Helpers;
using AdventureWorks.SkiResort.Infrastructure.Model;
using Microsoft.Data.Entity;

namespace AdventureWorks.SkiResort.Infrastructure.Repositories
{
    public class LiftsRepository
    {
        SkiResortContext _context;

        public LiftsRepository(SkiResortContext context)
        {
            _context = context;
        }

        public async Task<Lift> GetAsync(int id)
        {
            return await _context.Lifts.SingleOrDefaultAsync(l => l.LiftId == id);
        }

        public async Task<IEnumerable<Lift>> GetNearByAsync(double latitude, double longitude)
        {
            return await _context.Lifts
                .OrderBy(l => MathCoordinates.GetDistance(l.Latitude, l.Longitude, latitude, longitude, 'M'))
                .ToListAsync();
        }
    }
}
