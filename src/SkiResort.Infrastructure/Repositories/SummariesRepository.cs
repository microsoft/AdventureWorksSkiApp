
using AdventureWorks.SkiResort.Infrastructure.Context;
using System.Threading.Tasks;
using AdventureWorks.SkiResort.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AdventureWorks.SkiResort.Infrastructure.Repositories
{
    public class SummariesRepository
    {
        SkiResortContext _context;

        public SummariesRepository(SkiResortContext context)
        {
            _context = context;
        }

        public async Task<Summary> GetAsync()
        {
            var today = DateTime.UtcNow.Date;
            var summary =  await _context.Summaries
                .FirstOrDefaultAsync(s =>
                    s.DateTime.Year == today.Year &&
                    s.DateTime.Month == today.Month &&
                    s.DateTime.Day == today.Day &&
                    s.DateTime.Hour == today.Hour);

            if (summary == null)
                summary = await GetDefaultSummary();

            return summary;
        }

        async Task<Summary> GetDefaultSummary()
        {
            return await _context.Summaries.OrderByDescending(s => s.SummaryId).FirstOrDefaultAsync();
        }

    }
}
