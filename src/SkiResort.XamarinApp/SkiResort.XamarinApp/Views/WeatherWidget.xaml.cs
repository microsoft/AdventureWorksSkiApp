using SkiResort.XamarinApp.ViewModels;
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
            BindingContext = new WeatherWidgetViewModel();
            InitializeComponent();
        }
    }
}
