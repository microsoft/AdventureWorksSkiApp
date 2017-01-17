using SkiResort.XamarinApp.Entities;
using System;
using System.Collections.Generic;
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
        public DiningDetailViewModel(Restaurant restaurant)
        {
            Restaurant = restaurant;
        }
    }
}
