using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Views
{
    public partial class WeatherWidgetItem : ContentView
    {
        public static readonly BindableProperty IconProperty = BindableProperty.Create("Icon", typeof(string), typeof(WeatherWidgetItem), null);
        public string Icon {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public string Title { get; set; }
        public string Value { get; set; }
        public WeatherWidgetItem()
        {
            InitializeComponent();
        }
    }
}
