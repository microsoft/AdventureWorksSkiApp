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
            Detail = CreateDetail(typeof(HomePage));

            mainMenu.ListView.ItemSelected += OnItemSelected;
        }

        Page CreateDetail(Type pageType)
        {
            var style = pageType == typeof(HomePage) ?
                CustomNavigationPageStyle.Black :
                CustomNavigationPageStyle.Blue;

            return new CustomNavigationPage((Page)Activator.CreateInstance(pageType), style);
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuItem;
            if (item != null && item.TargetType != null)
            {
                Detail = CreateDetail(item.TargetType);
            }
            CloseMenu();
        }

        void CloseMenu()
        {
            ((MainMenu)Master).ListView.SelectedItem = null;
            IsPresented = false;
        }
    }
}
