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

        private Lift selectedLift { get; set; }
        public Lift SelectedLift
        {
            get
            {
                return selectedLift;
            }
            set
            {
                if (value != selectedLift)
                {
                    NavigationService.Instance.NavigateTo(typeof(LiftDetailViewModel), value);
                    selectedLift = null;
                    OnPropertyChanged("SelectedLift");
                }
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
