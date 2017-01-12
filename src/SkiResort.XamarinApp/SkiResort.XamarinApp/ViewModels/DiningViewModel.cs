using SkiResort.XamarinApp.Entities;
using SkiResort.XamarinApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort.XamarinApp.ViewModels
{
    class DiningViewModel : BaseViewModel
    {
        private ObservableCollection<Restaurant> restaurants { set; get; }
        public ObservableCollection<Restaurant> Restaurants
        {
            get
            {
                return restaurants;
            }
            set
            {
                restaurants = value;
                OnPropertyChanged("Restaurants");
            }
        }

        private bool loading { set; get; }
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

        private Geolocation userPosition { set; get; }
        public Geolocation UserPosition
        {
            get { return userPosition; }
            set
            {
                if (userPosition != value)
                {
                    userPosition = value;
                    OnPropertyChanged("UserPosition");
                }
            }
        }

        public DiningViewModel()
        {
            Restaurants = new ObservableCollection<Restaurant>();
            UserPosition = new Geolocation { Latitude = 0, Longitude = 0 };
            FetchUserPosition();
            FetchRestaurants();
        }

        private async void FetchUserPosition() {
            //var locator = CrossGeolocator.Current;
            //locator.DesiredAccuracy = 100;
            //var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            //if (position != null)
            //{
            //    UserPosition = new Geolocation
            //    {
            //        Latitude = position.Latitude,
            //        Longitude = position.Longitude
            //    };
            //}
        }

        private async void FetchRestaurants()
        {
            Loading = true;

            var restaurantsService = new RestaurantsService();
            var restaurants = await restaurantsService.GetRestaurants();

            foreach(var restaurant in restaurants)
            {
                Restaurants.Add(restaurant);
            }

            Loading = false;
        }
    }
}
