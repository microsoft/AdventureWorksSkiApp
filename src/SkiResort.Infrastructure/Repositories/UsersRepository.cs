using System;
using AdventureWorks.SkiResort.Infrastructure.Context;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using AdventureWorks.SkiResort.Infrastructure.Model;

namespace AdventureWorks.SkiResort.Infrastructure.Repositories
{
    public class UsersRepository
    {
        SkiResortContext _context;

        public UsersRepository(SkiResortContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetUserAsync(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);

            if (user != null)
            {
                return new ApplicationUser()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Photo = user.Photo,
                };
            }

            return null;
        }
        
        public async Task<byte[]> GetPhotoAsync(string userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

            if (user != null)
                return user.Photo;

            return null;
        }
    }
}
