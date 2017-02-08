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
    class LoginViewModel : BaseViewModel
    {
        #region Properties
        private string username { get; set; }
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged("Username");
                OnPropertyChanged("LoginButtonEnabled");
            }
        }

        private string password { get; set; }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("Password");
                OnPropertyChanged("LoginButtonEnabled");
            }
        }

        private bool loading { get; set; }
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
                OnPropertyChanged("InputEnabled");
                OnPropertyChanged("LoginButtonEnabled");
            }
        }

        private bool loginFailed { get; set; }
        public bool LoginFailed
        {
            get
            {
                return loginFailed;
            }
            set
            {
                loginFailed = value;
                OnPropertyChanged("LoginFailed");
            }
        }

        public bool LoginButtonEnabled
        {
            get
            {
                return (!Loading && Username != "" && Password != "");
            }
            set { }
        }

        public bool InputEnabled
        {
            get
            {
                return (!Loading);
            }
            set { }
        }
        #endregion

        #region Commands
        public ICommand ClickLoginCommand => new Command(clickLoginCommandHandler);
        private async void clickLoginCommandHandler()
        {
            Loading = true;
            LoginFailed = false;
            var success = await _authService.Login(Username, Password);
            if (success)
            {
                resetData();
                await _navigationService.NavigateBack();
            }
            else
            {
                LoginFailed = true;
                Loading = false;
            }
        }
        #endregion

        #region Dependencies
        private NavigationService _navigationService;
        private AuthService _authService;
        #endregion

        public LoginViewModel()
        {
            _authService = AuthService.Instance;
            _navigationService = NavigationService.Instance;
        }

        private void resetData()
        {
            Username = "";
            Password = "";
            Loading = false;
        }
    }
}
