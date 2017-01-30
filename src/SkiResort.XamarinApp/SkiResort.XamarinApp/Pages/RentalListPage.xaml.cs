using SkiResort.XamarinApp.Entities;
using SkiResort.XamarinApp.Interfaces;
using SkiResort.XamarinApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Pages
{
    public partial class RentalListPage : ContentPage, IBarTint
    {
        public RentalListPage()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Icon = "list-ul-menuicon.png";
            }
            InitializeComponent();
        }

        public Color GetBarBackgroundColor() => Color.FromHex("#15719E");
        public Color GetBarTextColor() => Color.FromHex("#FFFFFF");
    }
}
