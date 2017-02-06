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
        #region Properties
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

        public bool ShowLogoutButton
        {
            get
            {
                return _authService.User != null;
            }
            set { }
        }
        #endregion

        #region Commands
        public ICommand ClickLogoutCommand => new Command(clickLogoutCommandHandler);
        private void clickLogoutCommandHandler()
        {
            _authService.Logout();
            MessagingCenter.Send(this, "LogoutButtonClicked");
        }
        public ICommand ItemSelectedCommand => new Command<MainMenuItem>(itemSelectedCommandHandler);
        private void itemSelectedCommandHandler(MainMenuItem item)
        {
            MessagingCenter.Send(this, "MainMenuItemSelected", item);
        }
        #endregion

        #region Dependencies
        private AuthService _authService { get; set; }
        #endregion

        public MainMenuViewModel ()
        {
            _authService = AuthService.Instance;

            MessagingCenter.Subscribe<AuthService>(this, "UserChanged", (sender) => {
                OnPropertyChanged("ShowLogoutButton");
            });

            initializeOptions();
        }

        private void initializeOptions()
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
