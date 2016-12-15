using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Pages
{
    public partial class LiftStatusPage : ContentPage
    {
        public LiftStatusPage()
        {
            InitializeComponent();

            var liftItems = new List<LiftGroup>()
            {
                new LiftGroup("Open Lifts", "O")
                {
                    new LiftItem {Name = "Bear Creek" },
                    new LiftItem {Name = "Overlake jump" },
                    new LiftItem {Name = "Grass Lawn Caf" },
                    new LiftItem {Name = "Education Hill" },
                    new LiftItem {Name = "Belltown Express" },
                },
                new LiftGroup("Closed Lifts", "B")
                {
                    new LiftItem {Name = "Redmond Way" },
                    new LiftItem {Name = "Borealis" },
                    new LiftItem {Name = "Bear Creek" },
                }
            };

            listView.ItemsSource = liftItems;
        }
    }

    public class LiftItem
    {
        public string Name { get; set; }
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
