using SkiResort.XamarinApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using SkiResort.XamarinApp.Entities;
using SkiResort.XamarinApp.ViewModels;
using System.Collections.ObjectModel;

namespace SkiResort.XamarinApp.Pages
{
    public partial class LiftStatusPage : ContentPage, IBarTint
    {
        public LiftStatusPage()
        {
            BindingContext = new LiftStatusViewModel();
            InitializeComponent();
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
}
