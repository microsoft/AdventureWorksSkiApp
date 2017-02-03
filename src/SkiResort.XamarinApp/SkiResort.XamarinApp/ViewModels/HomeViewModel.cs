using SkiResort.XamarinApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort.XamarinApp.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            TryLogin();
        }

        private async void TryLogin()
        {
            await AuthService.Instance.Login("a", "a");
        }
    }
}
