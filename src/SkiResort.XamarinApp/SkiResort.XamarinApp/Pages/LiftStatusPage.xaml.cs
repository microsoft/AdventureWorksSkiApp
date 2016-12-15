using SkiResort.XamarinApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Pages
{
    public partial class LiftStatusPage : ContentPage, IBarTint
    {
        public LiftStatusPage()
        {
            InitializeComponent();

            var liftItems = new List<LiftGroup>()
            {
                new LiftGroup("Open Lifts", "O")
                {
                    new LiftItem {Name = "Bear Creek", IconSource="liftStatus_square.png" },
                    new LiftItem {Name = "Overlake jump", IconSource="liftStatus_square.png" },
                    new LiftItem {Name = "Grass Lawn Caf", IconSource="liftStatus_circle.png" },
                    new LiftItem {Name = "Education Hill", IconSource="liftStatus_diamond.png" },
                    new LiftItem {Name = "Belltown Express", IconSource="liftStatus_circle.png" },
                },
                new LiftGroup("Closed Lifts", "B")
                {
                    new LiftItem {Name = "Redmond Way", IconSource="liftStatus_diamond.png" },
                    new LiftItem {Name = "Borealis", IconSource="liftStatus_square.png" },
                    new LiftItem {Name = "Bear Creek", IconSource="liftStatus_square.png" },
                }
            };

            listView.ItemsSource = liftItems;
        }

        public Color GetBarBackgroundColor()
        {
            return Color.FromHex("#15719E");
        }
        public Color GetBarTextColor()
        {
            return Color.FromHex("#FFFFFF");
        }
    }

    public class LiftItem
    {
        public string Name { get; set; }
        public string IconSource { get; set; }
    }

    public class LiftGroup : List<LiftItem>
    {
        public string Title { get; set; }
        public string ShortName { get; set; }
        public LiftGroup(string title, string shortName)
        {
            Title = title;
            ShortName = shortName;
        }

        public static IList<LiftGroup> All { private set; get; }
    }
}
