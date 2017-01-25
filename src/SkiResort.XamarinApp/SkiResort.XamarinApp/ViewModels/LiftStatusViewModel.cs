using SkiResort.XamarinApp.Entities;
using SkiResort.XamarinApp.Pages;
using SkiResort.XamarinApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SkiResort.XamarinApp.ViewModels
{
    class LiftStatusViewModel : BaseViewModel
    {
        private ObservableCollection<LiftGroup> liftGroups { set; get; }
        public ObservableCollection<LiftGroup> LiftGroups
        {
            get
            {
                return liftGroups;
            }
            set
            {
                liftGroups = value;
                OnPropertyChanged("LiftGroups");
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

        public ICommand ItemSelectedCommand => new Command<Lift>(OnSelectItem);

        private async void OnSelectItem(Lift obj)
        {
            await NavigationService.Instance.NavigateTo(typeof(LiftDetailViewModel), obj);
        }

        public LiftStatusViewModel()
        {
            LiftGroups = new ObservableCollection<LiftGroup>();
            FetchLiftStatus();
        }

        private async void FetchLiftStatus()
        {
            Loading = true;
            var liftService = new LiftService();
            var lifts = await liftService.GetLifts();

            var openLifts = new LiftGroup("Open Lifts", "O");
            var closedLifts = new LiftGroup("Closed Lifts", "C");

            foreach(var lift in lifts)
            {
                if (lift.Status == LiftStatus.Open)
                    openLifts.Add(lift);
                else if (lift.Status == LiftStatus.Closed)
                    closedLifts.Add(lift);
            }

            LiftGroups.Add(openLifts);
            LiftGroups.Add(closedLifts);
            Loading = false;
        }
    }
}
