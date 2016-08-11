using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AdventureWorks.SkiResort.Infrastructure.Model
{
    public class ApplicationUser : IdentityUser
    {
        public byte[] Photo { get; set; }

        public string FullName { get; set; }
    }
}
