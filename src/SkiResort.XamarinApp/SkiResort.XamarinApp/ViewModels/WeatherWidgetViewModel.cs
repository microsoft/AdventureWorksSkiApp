using SkiResort.XamarinApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp.ViewModels
{
    class WeatherWidgetViewModel : BaseViewModel
    {
        string weather, temperature, wind, baseDepth;
        double itemsOpacity = 0.2;
        bool loading = true;

        public string Weather
        {
            get { return weather; }
            set {
                if (weather != value)
                {
                    weather = value;
                    OnPropertyChanged("Weather");
                }
            }
        }

        public string Temperature
        {
            get { return temperature; }
            set
            {
                if (temperature != value)
                {
                    temperature = value;
                    OnPropertyChanged("Temperature");
                }
            }
        }

        public string Wind
        {
            get { return wind; }
            set
            {
                if (wind != value)
                {
                    wind = value;
                    OnPropertyChanged("Wind");
                }
            }
        }

        public string BaseDepth
        {
            get { return baseDepth; }
            set
            {
                if (baseDepth != value)
                {
                    baseDepth = value;
                    OnPropertyChanged("BaseDepth");
                }
            }
        }

        public double ItemsOpacity
        {
            get { return itemsOpacity; }
            set
            {
                if (itemsOpacity != value)
                {
                    itemsOpacity = value;
                    OnPropertyChanged("ItemsOpacity");
                }
            }
        }

        public bool Loading
        {
            get { return loading; }
            set
            {
                if (loading != value)
                {
                    loading = value;
                    OnPropertyChanged("Loading");
                }
            }
        }

        public WeatherWidgetViewModel()
        {
            FetchWeatherSummary();
        }

        private async void FetchWeatherSummary()
        {
            var weatherService = new WeatherService();
            var weatherSummary = await weatherService.GetSummary();

            if (weatherSummary != null)
            {
                Weather = "Snowing";
                Temperature = weatherSummary.MinTemperature + "/" + weatherSummary.MaxTemperature + "º F";
                Wind = weatherSummary.Wind + " mph";
                BaseDepth = weatherSummary.BaseDepth + " inch";
            }

            ItemsOpacity = 1;
            Loading = false;
        }
    }
}
