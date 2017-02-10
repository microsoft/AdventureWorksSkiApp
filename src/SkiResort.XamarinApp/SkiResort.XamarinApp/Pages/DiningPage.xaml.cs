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
    public partial class DiningPage : ContentPage, IBarTint
    {
        public DiningPage()
        {
            InitializeComponent();
        }

        public Color GetBarBackgroundColor() => Config.BLUE_BAR_COLOR;
        public Color GetBarTextColor() => Config.DEFAULT_BAR_TEXT_COLOR;
    }
}
