using SkiResort.XamarinApp.Interfaces;
using SkiResort.XamarinApp.Services;
using SkiResort.XamarinApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Pages
{
    public class RentalPage : TabbedPage, IBarTint
    {
        public RentalPage()
        {
            BarBackgroundColor = Color.FromHex("#1A90C9");
            Title = "Rental Reservation";
            Children.Add(NavigationService.Instance.CreatePage(typeof(RentalListViewModel)));
            Children.Add(NavigationService.Instance.CreatePage(typeof(RentalFormViewModel)));
        }

        public Color GetBarBackgroundColor() => Color.FromHex("#15719E");
        public Color GetBarTextColor() => Color.FromHex("#FFFFFF");
    }
}
