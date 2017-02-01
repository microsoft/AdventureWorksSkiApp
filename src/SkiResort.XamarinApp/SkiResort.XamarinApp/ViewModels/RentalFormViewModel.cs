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
    class RentalFormViewModel : BaseViewModel
    {
        #region Properties
        private DateTimeOffset startDate { set; get; }
        public string StartDate
        {
            get
            {
                return startDate.ToString();
            }
            set
            {
                startDate = DateTimeOffset.Parse(value);
                OnPropertyChanged("StartDate");
                OnPropertyChanged("CanSave");
                OnPropertyChanged("TotalCost");
                OnPropertyChanged("SaveButtonBackgroundColor");
            }
        }

        private DateTimeOffset endDate { set; get; }
        public string EndDate
        {
            get
            {
                return endDate.ToString();
            }
            set
            {
                endDate = DateTimeOffset.Parse(value);
                OnPropertyChanged("EndDate");
                OnPropertyChanged("CanSave");
                OnPropertyChanged("TotalCost");
                OnPropertyChanged("SaveButtonBackgroundColor");
            }
        }

        private ObservableCollection<RentalActivity> rentalActivityOptions { set; get; }
        public ObservableCollection<RentalActivity> RentalActivityOptions
        {
            get
            {
                return rentalActivityOptions;
            }
            set
            {
                rentalActivityOptions = value;
                OnPropertyChanged("RentalActivityOptions");
            }
        }

        private RentalActivity selectedRentalActivity { set; get; }
        public RentalActivity SelectedRentalActivity
        {
            get
            {
                return selectedRentalActivity;
            }
            set
            {
                selectedRentalActivity = value;
                OnPropertyChanged("SelectedRentalActivity");
            }
        }

        private ObservableCollection<PickerItem<TimeSpan>> pickUpTimeOptions { set; get; }
        public ObservableCollection<PickerItem<TimeSpan>> PickUpTimeOptions
        {
            get
            {
                return pickUpTimeOptions;
            }
            set
            {
                pickUpTimeOptions = value;
                OnPropertyChanged("PickUpTimeOptions");
            }
        }

        private PickerItem<TimeSpan> selectedPickUpTime { set; get; }
        public PickerItem<TimeSpan> SelectedPickUpTime
        {
            get
            {
                return selectedPickUpTime;
            }
            set
            {
                selectedPickUpTime = value;
                OnPropertyChanged("SelectedPickUpTime");
            }
        }

        private ObservableCollection<RentalCategory> rentalCategoryOptions { set; get; }
        public ObservableCollection<RentalCategory> RentalCategoryOptions
        {
            get
            {
                return rentalCategoryOptions;
            }
            set
            {
                rentalCategoryOptions = value;
                OnPropertyChanged("RentalCategoryOptions");
            }
        }

        private RentalCategory selectedRentalCategory { set; get; }
        public RentalCategory SelectedRentalCategory
        {
            get
            {
                return selectedRentalCategory;
            }
            set
            {
                selectedRentalCategory = value;
                OnPropertyChanged("SelectedRentalCategory");
            }
        }

        private ObservableCollection<PickerItem<double>> shoeSizeOptions { set; get; }
        public ObservableCollection<PickerItem<double>> ShoeSizeOptions
        {
            get
            {
                return shoeSizeOptions;
            }
            set
            {
                shoeSizeOptions = value;
                OnPropertyChanged("ShoeSizeOptions");
            }
        }

        private PickerItem<double> selectedShoeSize { get; set; }
        public PickerItem<double> SelectedShoeSize
        {
            get
            {
                return selectedShoeSize;
            }
            set
            {
                selectedShoeSize = value;
                OnPropertyChanged("SelectedShoeSize");
            }
        }

        private ObservableCollection<PickerItem<int>> skiSizeOptions { set; get; }
        public ObservableCollection<PickerItem<int>> SkiSizeOptions
        {
            get
            {
                return skiSizeOptions;
            }
            set
            {
                skiSizeOptions = value;
                OnPropertyChanged("SkiSizeOptions");
            }
        }

        private PickerItem<int> selectedSkiSize { get; set; }
        public PickerItem<int> SelectedSkiSize
        {
            get
            {
                return selectedSkiSize;
            }
            set
            {
                selectedSkiSize = value;
                OnPropertyChanged("SelectedSkiSize");
            }
        }

        private ObservableCollection<PickerItem<int>> poleSizeOptions { set; get; }
        public ObservableCollection<PickerItem<int>> PoleSizeOptions
        {
            get
            {
                return poleSizeOptions;
            }
            set
            {
                poleSizeOptions = value;
                OnPropertyChanged("PoleSizeOptions");
            }
        }

        private PickerItem<int> selectedPoleSize { get; set; }
        public PickerItem<int> SelectedPoleSize
        {
            get
            {
                return selectedPoleSize;
            }
            set
            {
                selectedPoleSize = value;
                OnPropertyChanged("SelectedPoleSize");
            }
        }

        private RentalGoal selectedRentalGoal { get; set; }

        public string DemoOptionBackgroundColor
        {
            get
            {
                return getBackgroundForRentalGoal(RentalGoal.Demo);
            }
            set { }
        }

        public string PerformanceOptionBackgroundColor
        {
            get
            {
                return getBackgroundForRentalGoal(RentalGoal.Performance);
            }
            set { }
        }

        public string SportOptionBackgroundColor
        {
            get
            {
                return getBackgroundForRentalGoal(RentalGoal.Sport);
            }
            set { }
        }

        public bool CanSave
        {
            get
            {
                if ((endDate - startDate).TotalDays < 0)
                    return false;
                return true;
            }
            set { }
        }

        public double TotalCost
        {
            get
            {
                double result = 0;
                if (CanSave)
                {
                    result = 20 + ((endDate - startDate).TotalDays * 5);
                }
                return result;
            }
            set { }
        }

        public string SaveButtonBackgroundColor
        {
            get
            {
                return CanSave ? "#1A90C9" : "#F1F1F1";
            }
        }

        private bool loading { set; get; }
        public bool Loading
        {
            get
            {
                return loading;
            }
            set
            {
                loading = value;
                OnPropertyChanged("Loading");
                OnPropertyChanged("ContentOpacity");
            }
        }

        public float ContentOpacity
        {
            get
            {
                return Loading ? 0.3F : 1F;
            }
            set { }
        }
        #endregion

        #region Commands
        public ICommand ClickGoalOptionCommand { get; set; }
        public ICommand ClickSaveCommand { get; set; }
        #endregion

        public RentalFormViewModel()
        {
            initializeData();

            ClickGoalOptionCommand = new Command<string>(ClickGoalOptionCommandHandler);
            ClickSaveCommand = new Command(ClickSaveCommandHandler);
        }

        void ClickGoalOptionCommandHandler(string rentalGoalName)
        {
            selectedRentalGoal = getRentalGoalFromName(rentalGoalName);
            OnPropertyChanged("DemoOptionBackgroundColor");
            OnPropertyChanged("PerformanceOptionBackgroundColor");
            OnPropertyChanged("SportOptionBackgroundColor");
        }

        async void ClickSaveCommandHandler()
        {
            Loading = true;
            var completeStartDate = startDate;
            completeStartDate = completeStartDate.Date.Add(SelectedPickUpTime.Value);
            var rental = new Rental()
            {
                StartDate = completeStartDate,
                EndDate = endDate,
                PickupHour = 0,
                Category = SelectedRentalCategory,
                Activity = SelectedRentalActivity,
                Goal = selectedRentalGoal,
                ShoeSize = SelectedShoeSize.Value,
                SkiSize = SelectedSkiSize.Value,
                PoleSize = SelectedPoleSize.Value,
                TotalCost = TotalCost,
            };

            var rentalService = new RentalService();
            await rentalService.SaveRental(rental);
            MessagingCenter.Send(this, "SetRentalListTab");
            initializeData();
            Loading = false;
        }

        RentalGoal getRentalGoalFromName(string rentalGoalName) {
            RentalGoal matchingRentalGoal = RentalGoal.Demo;
            switch(rentalGoalName)
            {
                case "Demo":
                    matchingRentalGoal = RentalGoal.Demo;
                    break;
                case "Performance":
                    matchingRentalGoal = RentalGoal.Performance;
                    break;
                case "Sport":
                    matchingRentalGoal = RentalGoal.Sport;
                    break;
            }
            return matchingRentalGoal;
        }

        string getBackgroundForRentalGoal(RentalGoal rentalGoal) {
            return selectedRentalGoal == rentalGoal ? "#1A90C9" : "#323232";
        }

        #region Data Initializers

        void initializeData() {
            Loading = false;
            initializeDatePeriod();
            RentalActivityOptions = new ObservableCollection<RentalActivity>
            {
                RentalActivity.Ski,
                RentalActivity.Snowboard,
            };
            SelectedRentalActivity = RentalActivity.Ski;
            RentalCategoryOptions = new ObservableCollection<RentalCategory>
            {
                RentalCategory.Beginner,
                RentalCategory.Intermediate,
                RentalCategory.Advanced
            };
            SelectedRentalCategory = RentalCategory.Beginner;
            selectedRentalGoal = RentalGoal.Demo;
            initializePickUpHoursOptions();
            initializeShoeSizeOptions();
            initializeSkiSizeOptions();
            initializePoleSizeOptions();
        }

        void initializeDatePeriod()
        {
            StartDate = DateTimeOffset.Now.Date.ToString();
            EndDate = DateTimeOffset.Now.Date.ToString();
        }

        void initializePickUpHoursOptions()
        {
            PickUpTimeOptions = new ObservableCollection<PickerItem<TimeSpan>>();
            int nOfOptions = 57;
            int startHour = 6;
            int startMinute = 0;
            for(var i = 0; i < nOfOptions; i++)
            {
                double hour = (startHour + Math.Floor((double)((i * 15) / 60))) % 24;
                double minute = (startMinute + (double)((i * 15))) % 60;
                string meridiem = hour >= 12 ? "pm" : "am";

                var timeSpan = new TimeSpan((int)hour, (int)minute, 0);

                PickUpTimeOptions.Add(new PickerItem<TimeSpan> {
                    Value = timeSpan,
                    Text = string.Format("{0:00}:{1:00} {2}", hour % 12, minute, meridiem)
                });
            }
            SelectedPickUpTime = PickUpTimeOptions[0];
        }

        void initializeShoeSizeOptions()
        {
            ShoeSizeOptions = new ObservableCollection<PickerItem<double>>() {};

            for (var i = 1; i <= 3; i++)
            {
                ShoeSizeOptions.Add(new PickerItem<double> { Value = i, Text = i.ToString() });
            }

            double minShoeSize = 4;
            double maxShoeSize = 16.5;
            double step = 0.5;
            for(var i = minShoeSize; i <= maxShoeSize; i += step)
            {
                ShoeSizeOptions.Add(new PickerItem<double>
                {
                    Value = i,
                    Text = i.ToString()
                });
            }
            SelectedShoeSize = ShoeSizeOptions[0];
        }

        void initializeSkiSizeOptions()
        {
            SkiSizeOptions = new ObservableCollection<PickerItem<int>>();
            int minSkiSize = 115;
            int maxSkiSize = 200;
            for (var i = minSkiSize; i <= maxSkiSize; i++)
            {
                SkiSizeOptions.Add(new PickerItem<int>
                {
                    Value = i,
                    Text = string.Format("{0} in", i)
                });
            }
            SelectedSkiSize = SkiSizeOptions[0];
        }

        void initializePoleSizeOptions()
        {
            PoleSizeOptions = new ObservableCollection<PickerItem<int>>();
            int minPoleSize = 32;
            int maxPoleSize = 57;
            for (var i = minPoleSize; i <= maxPoleSize; i++)
            {
                PoleSizeOptions.Add(new PickerItem<int>
                {
                    Value = i,
                    Text = string.Format("{0} in", i)
                });
            }
            SelectedPoleSize = PoleSizeOptions[0];
        }

        #endregion

        public struct PickerItem<T>
        {
            public T Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }
    }

}
