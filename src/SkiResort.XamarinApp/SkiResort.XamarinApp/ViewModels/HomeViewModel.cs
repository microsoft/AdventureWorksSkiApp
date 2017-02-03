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
        public ICommand ClickLoginCommand { get; set; }
        public HomeViewModel()
        {
            ClickLoginCommand = new Command(ClickLoginCommandHandler);
        }

        private async void ClickLoginCommandHandler()
        {
            await NavigationService.Instance.NavigateTo(typeof(LoginViewModel));
        }
    }
}
