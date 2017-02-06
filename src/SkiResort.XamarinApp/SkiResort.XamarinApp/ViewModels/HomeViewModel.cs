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

        private AuthService _authService;

        public ICommand ClickLoginCommand { get; set; }
        public HomeViewModel()
        {
            _authService = AuthService.Instance;
            User = _authService.User;
            ClickLoginCommand = new Command(ClickLoginCommandHandler);

            MessagingCenter.Subscribe<AuthService>(this, "HomeRefreshUser", (sender) => {
                User = _authService.User;
            });
        }

        private async void ClickLoginCommandHandler()
        {
            await NavigationService.Instance.NavigateTo(typeof(LoginViewModel));
        }
    }
}
