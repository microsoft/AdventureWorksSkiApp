using SkiResort.XamarinApp.Entities;
using SkiResort.XamarinApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Pages
{
    public partial class RentalPage : ContentPage, IBarTint
    {
        public RentalPage()
        {
            InitializeComponent();
            listView.ItemsSource = new List<Rental>()
            {
                new Rental() {RentalId = 3 },
                new Rental() {RentalId = 4 }
            };
        }

        public Color GetBarBackgroundColor() => Color.FromHex("#15719E");
        public Color GetBarTextColor() => Color.FromHex("#FFFFFF");
    }
}
