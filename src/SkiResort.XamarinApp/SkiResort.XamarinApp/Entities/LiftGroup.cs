using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort.XamarinApp.Entities
{
    public class LiftGroup : List<Lift>
    {
        public string Title { get; set; }
        public string IconPath { get; set; }
        public string ShortName { get; set; }
        public LiftGroup(string title, string shortName, string iconPath)
        {
            Title = title;
            ShortName = shortName;
            IconPath = iconPath;
        }

        public static IList<LiftGroup> All { private set; get; }
    }
}
