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
            InitializeComponent();
        }

        public Color GetBarBackgroundColor() => Color.FromHex("#15719E");
        public Color GetBarTextColor() => Color.FromHex("#FFFFFF");
    }
}
