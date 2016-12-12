using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Views
{
    public partial class WeatherWidget : ContentView
    {
        public WeatherWidget()
        {
            InitializeComponent();
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.8);
        }
    }
}
