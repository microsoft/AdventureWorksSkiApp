using SkiResort.XamarinApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SkiResort.XamarinApp.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        #region Properties
        private User user { get; set; }
        public User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                OnPropertyChanged("User");
                OnPropertyChanged("UserExists");
                OnPropertyChanged("ShowLoginButton");
            }
        }

        public bool UserExists
        {
            get
            {
                return user != null;
            }
            set { }
        }

        public bool ShowLoginButton
        {
            get
            {
                return !UserExists;
            }
            set { }
        }
        #endregion

        #region Commands
        public ICommand ClickLoginCommand => new Command(clickLoginCommandHandler);
        private async void clickLoginCommandHandler()
        {
            await NavigationService.Instance.NavigateTo(typeof(LoginViewModel));
        }
        #endregion

        #region Dependencies
        private AuthService _authService;
        #endregion

        public HomeViewModel()
        {
            _authService = AuthService.Instance;
            User = _authService.User;

            MessagingCenter.Subscribe<AuthService>(this, "UserChanged", (sender) => {
                User = _authService.User;
            });
        }
    }
}
