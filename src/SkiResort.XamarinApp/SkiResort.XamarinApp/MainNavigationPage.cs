using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp
{
    public class MainNavigationPage : NavigationPage
    {
        private ContentPage contentPage;

        public MainNavigationPage()
        {
            contentPage = new MainPage();
            BarBackgroundColor = Color.FromHex("#141414");
            SetValue(BarTextColorProperty, Color.White);
            SetTitleIcon(contentPage, "logo.png");
            AddContent();
        }

        public async Task AddContent()
        {
            await PushAsync(contentPage);
            // NavigationPage.SetHasNavigationBar(this, false); 
        }
    }
}
