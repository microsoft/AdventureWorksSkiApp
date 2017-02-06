using SkiResort.XamarinApp.Entities;
using SkiResort.XamarinApp.Pages;
using SkiResort.XamarinApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp.ViewModels
{
    class RentalListViewModel : BaseViewModel
    {
        #region Properties
        private ObservableCollection<Rental> rentals { set; get; }
        public ObservableCollection<Rental> Rentals
        {
            get
            {
                return rentals;
            }
            set
            {
                rentals = value;
                OnPropertyChanged("Rentals");
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
        #endregion

        public RentalListViewModel()
        {
            fetchRentals();

            MessagingCenter.Subscribe<RentalPage>(this, "Refresh", (sender) => {
                fetchRentals();
            });
        }

        private async void fetchRentals()
        {
            Rentals = new ObservableCollection<Rental>();
            Loading = true;

            var rentalService = new RentalService();
            var rentals = await rentalService.GetRentals();

            foreach(var rental in rentals)
            {
                Rentals.Add(rental);
            }

            Loading = false;
        }
    }
}
