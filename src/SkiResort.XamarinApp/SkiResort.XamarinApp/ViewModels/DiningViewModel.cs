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
                OnPropertyChanged("FilteredRestaurants");
            }
        }
        public ObservableCollection<Restaurant> FilteredRestaurants
        {
            get
            {
                var result = restaurants
                    .Where(restaurantFilterer(SearchText))
                    .OrderBy((restaurant) =>
                    {
                        object orderBy = null;
                        switch (SelectedSortableOptionIndex)
                        {
                            case 0:
                                orderBy = restaurant.Rating;
                                break;
                            case 1:
                                orderBy = restaurant.LevelOfNoise;
                                break;
                            case 2:
                                orderBy = restaurant.PriceLevel;
                                break;
                            case 3:
                                // TODO
                                orderBy = restaurant.Name;
                                break;
                            case 4:
                                orderBy = restaurant.FamilyFriendly;
                                break;
                        }
                        return orderBy;
                    });

                return new ObservableCollection<Restaurant>(result);
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

        private string searchText { get; set; }
        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                if (value != searchText)
                {
                    searchText = value;
                    OnPropertyChanged("SearchText");
                    OnPropertyChanged("Restaurants");
                    OnPropertyChanged("FilteredRestaurants");
                }
            }
        }

        private List<string> sortableOptions { get; set; }
        public List<string> SortableOptions
        {
            get
            {
                return sortableOptions;
            }
            set
            {
                if (value != sortableOptions)
                {
                    sortableOptions = value;
                    OnPropertyChanged("SortByItems");
                }
            }
        }

        private int selectedSortableOptionIndex { get; set; }
        public int SelectedSortableOptionIndex
        {
            get
            {
                return selectedSortableOptionIndex;
            }
            set
            {
                if (value != selectedSortableOptionIndex)
                {
                    selectedSortableOptionIndex = value;
                    OnPropertyChanged("SelectedSortBy");
                    OnPropertyChanged("Restaurants");
                    OnPropertyChanged("FilteredRestaurants");
                }
            }
        }

        public DiningViewModel()
        {
            SearchText = "";
            SortableOptions = new List<string>
            {
                "Rating",
                "Level Noise",
                "Price",
                "Miles Away",
                "Family Friendly"
            };
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
            OnPropertyChanged("FilteredRestaurants");
        }

        private Func<Restaurant, bool> restaurantFilterer(string _searchText)
        {
            var loweredSearchText = _searchText.ToLower();

            return (restaurant) =>
            {
                if (loweredSearchText == "")
                    return true;
                var name = restaurant.Name.ToLower();
                return name.IndexOf(loweredSearchText) >= 0;
            };
        }
    }

}
