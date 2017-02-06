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
    
    class MainMenuViewModel : BaseViewModel
    {
        private ObservableCollection<MainMenuItem> mainMenuItems { get; set; }
        public ObservableCollection<MainMenuItem> MainMenuItems
        {
            get
            {
                return mainMenuItems;
            }
            set
            {
                mainMenuItems = value;
                OnPropertyChanged("MainMenuItems");
            }
        }

        public ICommand ClickLogoutCommand { get; set; }
        public ICommand ItemSelectedCommand { get; set; }

        private AuthService _authService { get; set; }

        public MainMenuViewModel ()
        {
            _authService = AuthService.Instance;

            ClickLogoutCommand = new Command(ClickLogoutCommandHandler);
            ItemSelectedCommand = new Command<MainMenuItem>(ItemSelectedCommandHandler);

            InitializeOptions();
        }

        private void ClickLogoutCommandHandler()
        {
            _authService.Logout();
            MessagingCenter.Send(this, "LogoutButtonClicked");
        }

        private void ItemSelectedCommandHandler(MainMenuItem item)
        {
            MessagingCenter.Send(this, "MainMenuItemSelected", item);
        }

        private void InitializeOptions()
        {
            MainMenuItems = new ObservableCollection<MainMenuItem>()
            {
                new MainMenuItem("Home", typeof(HomeViewModel)),
                new MainMenuItem("Lift Status", typeof(LiftStatusViewModel)),
                new MainMenuItem("Rental Reservation",typeof(RentalViewModel)),
                new MainMenuItem("Dining", typeof(DiningViewModel)),
                new MainMenuItem("Live Webcams", null),
                new MainMenuItem("Lift Tickets", null)
            };
        }

        public class MainMenuItem
        {
            public string Title { get; set; }
            public string IconSource { get; set; }
            public Type TargetType { get; set; }

            public MainMenuItem(string title, Type targetType)
            {
                Title = title;
                TargetType = targetType;
            }
        }
    }
}
