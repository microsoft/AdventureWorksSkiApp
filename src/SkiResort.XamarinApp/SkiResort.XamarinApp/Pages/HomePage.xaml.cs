using SkiResort.XamarinApp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp.Pages
{
    public partial class HomePage : ContentPage, IBarTint
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public Color GetBarBackgroundColor()
        {
            return Color.FromHex("#141414");
        }

        public Color GetBarTextColor()
        {
            return Color.FromHex("#FFFFFF");
        }
    }
}
