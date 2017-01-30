using SkiResort.XamarinApp.Entities;
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
        private ObservableCollection<string> skiOrSnowboardOptions { set; get; }
        public ObservableCollection<string> SkiOrSnowboardOptions
        {
            get
            {
                return skiOrSnowboardOptions;
            }
            set
            {
                skiOrSnowboardOptions = value;
                OnPropertyChanged("SkiOrSnowboardOptions");
            }
        }

        private ObservableCollection<string> pickUpTimeOptions { set; get; }
        public ObservableCollection<string> PickUpTimeOptions
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

        private ObservableCollection<string> rentalCategoryOptions { set; get; }
        public ObservableCollection<string> RentalCategoryOptions
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

        private ObservableCollection<string> shoeSizeOptions { set; get; }
        public ObservableCollection<string> ShoeSizeOptions
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

        private ObservableCollection<string> skiSizeOptions { set; get; }
        public ObservableCollection<string> SkiSizeOptions
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

        private ObservableCollection<string> poleSizeOptions { set; get; }
        public ObservableCollection<string> PoleSizeOptions
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
                return false;
            }
            set { }
        }
        #endregion

        #region Commands
        public ICommand ClickGoalOptionCommand { get; set; }
        #endregion

        public RentalFormViewModel()
        {
            SkiOrSnowboardOptions = new ObservableCollection<string>
            {
                "Ski",
                "Snowboard"
            };
            RentalCategoryOptions = new ObservableCollection<string>
            {
                "Beginner",
                "Intermediate",
                "Advanced"
            };
            selectedRentalGoal = RentalGoal.Performance;
            initializePickUpHoursOptions();
            initializeShoeSizeOptions();
            initializeSkiSizeOptions();
            initializePoleSizeOptions();

            ClickGoalOptionCommand = new Command<string>(ClickGoalOptionCommandHandler);
        }

        void ClickGoalOptionCommandHandler(string rentalGoalName)
        {
            selectedRentalGoal = getRentalGoalFromName(rentalGoalName);
            OnPropertyChanged("DemoOptionBackgroundColor");
            OnPropertyChanged("PerformanceOptionBackgroundColor");
            OnPropertyChanged("SportOptionBackgroundColor");
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

        void initializePickUpHoursOptions()
        {
            PickUpTimeOptions = new ObservableCollection<string>();
            int nOfOptions = 57;
            int startHour = 6;
            int startMinute = 0;
            for(var i = 0; i < nOfOptions; i++)
            {
                double hour = (startHour + Math.Floor((double)((i * 15) / 60))) % 24;
                double minute = (startMinute + (double)((i * 15))) % 60;
                string meridiem = hour >= 12 ? "pm" : "am";
                PickUpTimeOptions.Add(string.Format("{0:00}:{1:00} {2}", hour % 12, minute, meridiem));
            }
        }

        void initializeShoeSizeOptions()
        {
            ShoeSizeOptions = new ObservableCollection<string>() { "1", "2", "3" };
            double minShoeSize = 4;
            double maxShoeSize = 16.5;
            double step = 0.5;
            for(var i = minShoeSize; i <= maxShoeSize; i += step)
            {
                ShoeSizeOptions.Add(i.ToString());
            }
        }

        void initializeSkiSizeOptions()
        {
            SkiSizeOptions = new ObservableCollection<string>();
            int minSkiSize = 115;
            int maxSkiSize = 200;
            for (var i = minSkiSize; i <= maxSkiSize; i++)
            {
                SkiSizeOptions.Add(string.Format("{0} in", i));
            }
        }

        void initializePoleSizeOptions()
        {
            PoleSizeOptions = new ObservableCollection<string>();
            int minPoleSize = 32;
            int maxPoleSize = 57;
            for (var i = minPoleSize; i <= maxPoleSize; i++)
            {
                PoleSizeOptions.Add(string.Format("{0} in", i));
            }
        }
    }

}
