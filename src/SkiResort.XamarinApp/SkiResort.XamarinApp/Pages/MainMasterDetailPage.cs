using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp.Pages
{
    public class MainMasterDetailPage : MasterDetailPage
    {
        private MainMenu mainMenu;

        public MainMasterDetailPage()
        {
            mainMenu = new MainMenu();
            Master = mainMenu;
            Detail = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#141414"),
                BarTextColor = Color.FromHex("#FFFFFF")
            };
        }
    }
}
