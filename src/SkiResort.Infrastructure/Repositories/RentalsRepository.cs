
using AdventureWorks.SkiResort.Infrastructure.Context;
using AdventureWorks.SkiResort.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace AdventureWorks.SkiResort.Infrastructure.Repositories
{
    public class RentalsRepository
    {
        SkiResortContext _context;

        public RentalsRepository(SkiResortContext context)
        {
            _context = context;
        }

        public async Task<Rental> GetAsync(int id)
        {
            return await _context.Rentals.SingleOrDefaultAsync(r => r.RentalId == id);
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _context.Rentals
                .OrderBy(r => r.StartDate)
                .Where(r => r.StartDate >= DateTime.Today)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Rental rental)
        {
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
            return rental.RentalId;
        }

        public async Task UpdateAsync(Rental rental)
        {
            _context.Rentals.Update(rental);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rental = await _context.Rentals
                .SingleOrDefaultAsync(r => r.RentalId == id);

            if (rental != null)
            {
                _context.Rentals.Remove(rental);
                await _context.SaveChangesAsync();
            }
        }
    }
}
