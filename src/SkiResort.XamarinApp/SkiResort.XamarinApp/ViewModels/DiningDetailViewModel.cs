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
    class DiningDetailViewModel : BaseViewModel
    {
        private Restaurant restaurant { get; set; }
        public Restaurant Restaurant
        {
            get
            {
                return restaurant;
            }
            set
            {
                if (value != restaurant)
                {
                    restaurant = value;
                    OnPropertyChanged("Restaurant");
                }
            }
        }

        private ObservableCollection<Restaurant> recommendedRestaurants { get; set; }
        public ObservableCollection<Restaurant> RecommendedRestaurants
        {
            get
            {
                return recommendedRestaurants;
            }
            set
            {
                recommendedRestaurants = value;
                OnPropertyChanged("RecommendedRestaurants");
                OnPropertyChanged("ShowRecommendedRestaurants");
                OnPropertyChanged("FirstRecommendedRestaurant");
                OnPropertyChanged("SecondRecommendedRestaurant");
                OnPropertyChanged("ThirdRecommendedRestaurant");
            }
        }

        public bool ShowRecommendedRestaurants
        {
            get
            {
                return recommendedRestaurants.Count > 0;
            }
            set { }
        }

        public Restaurant FirstRecommendedRestaurant
        {
            get
            {
                return recommendedRestaurants.Count >= 1 ? recommendedRestaurants[0] : null;
            }
            set { }
        }

        public Restaurant SecondRecommendedRestaurant
        {
            get
            {
                return recommendedRestaurants.Count >= 2 ? recommendedRestaurants[1] : null;
            }
            set { }
        }

        public Restaurant ThirdRecommendedRestaurant
        {
            get
            {
                return recommendedRestaurants.Count >= 3 ? recommendedRestaurants[2] : null;
            }
            set { }
        }

        public DiningDetailViewModel(Restaurant restaurant)
        {
            Restaurant = restaurant;
            RecommendedRestaurants = new ObservableCollection<Restaurant>();

            FetchRecommendedRestaurants();
        }

        private async void FetchRecommendedRestaurants()
        {
            var restaurantsService = new RestaurantsService();
            RecommendedRestaurants = new ObservableCollection<Restaurant>(await restaurantsService.GetRecommendedRestaurants(Restaurant.RestaurantId));
        }
    }
}
