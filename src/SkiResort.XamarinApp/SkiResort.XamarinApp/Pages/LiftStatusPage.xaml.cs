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

            var liftItems = new List<LiftItem>()
            {
                new LiftItem {Name = "Ye" },
                new LiftItem {Name = "Ya" },
                new LiftItem {Name = "Yo" },
            };

            listView.ItemsSource = liftItems;
        }
    }

    public class LiftItem
    {
        public string Name { get; set; }
    }
}
