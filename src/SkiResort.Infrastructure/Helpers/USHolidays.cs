using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorks.SkiResort.Infrastructure.Helpers
{
    public class USHolidays
    {
        public static bool IsPublicHoliday(DateTime date)
        {
            // TODO: proper holiday handling. "PublicHoliday" NuGet package is great for this, but it doesn't yet offer a DNX version
            // Just handle new year's for now

            if ((date.Month == 1 && date.Day == 1) ||
                (date.Month == 1 && date.Day == 2 && date.DayOfWeek == DayOfWeek.Monday))
            {
                return true;
            }

            return false;
        }
    }
}
