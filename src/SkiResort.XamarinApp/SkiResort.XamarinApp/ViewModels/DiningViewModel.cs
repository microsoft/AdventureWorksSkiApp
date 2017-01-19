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

        private Restaurant selectedRestaurant { get; set; }
        public Restaurant SelectedRestaurant
        {
            get
            {
                return selectedRestaurant;
            }
            set
            {
                if (value != selectedRestaurant)
                {
                    NavigationService.Instance.NavigateTo(typeof(DiningDetailViewModel), value);
                    selectedRestaurant = null;
                    OnPropertyChanged("SelectedRestaurant");
                }
            }
        }

        public DiningViewModel()
        {
            Restaurants = new ObservableCollection<Restaurant>();
            FetchRestaurants();
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
