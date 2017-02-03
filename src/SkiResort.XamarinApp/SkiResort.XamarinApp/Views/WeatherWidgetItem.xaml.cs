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
        public static readonly BindableProperty IconProperty = BindableProperty.Create("Icon", typeof(string), typeof(WeatherWidgetItem), default(string));
        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(WeatherWidgetItem), default(string));
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(string), typeof(WeatherWidgetItem), default(string));

        public string Icon {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public WeatherWidgetItem()
        {
            InitializeComponent();
        }
    }
}
