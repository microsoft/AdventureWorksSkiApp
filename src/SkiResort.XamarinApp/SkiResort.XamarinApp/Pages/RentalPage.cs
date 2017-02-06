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
            BarBackgroundColor = Device.OS == TargetPlatform.iOS ? Config.DEFAULT_BAR_COLOR : Config.PRIMARY_COLOR;
            BarTextColor = Config.DEFAULT_BAR_TEXT_COLOR;

            Title = "Rental Reservation";

            var rentalListPage = NavigationService.Instance.CreatePage(typeof(RentalListViewModel));
            var rentalFormPage = NavigationService.Instance.CreatePage(typeof(RentalFormViewModel));

            Children.Add(rentalListPage);
            Children.Add(rentalFormPage);

            MessagingCenter.Subscribe<RentalFormViewModel>(this, "SetRentalListTab", (sender) => {
                SelectedItem = Children[0];
                MessagingCenter.Send(this, "Refresh");
            });
        }

        public Color GetBarBackgroundColor() => Config.BLUE_BAR_COLOR;
        public Color GetBarTextColor() => Config.DEFAULT_BAR_TEXT_COLOR;
    }
}
