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
        #region Properties
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
        #endregion

        #region Commands
        public ICommand ItemSelectedCommand => new Command<Lift>(onSelectItem);
        private async void onSelectItem(Lift obj)
        {
            await NavigationService.Instance.NavigateTo(typeof(LiftDetailViewModel), obj);
        }
        #endregion

        public LiftStatusViewModel()
        {
            LiftGroups = new ObservableCollection<LiftGroup>();
            fetchLiftStatus();
        }

        private async void fetchLiftStatus()
        {
            Loading = true;
            var liftService = new LiftService();
            var lifts = await liftService.GetLifts();

            var openLifts = new LiftGroup("Open Lifts", "O", "lift.png");
            var closedLifts = new LiftGroup("Closed Lifts", "C", "lift_closed.png");

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
