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

        private Dictionary<Type, Page> detailCache = new Dictionary<Type, Page>();

        public MainMasterDetailPage()
        {
            mainMenu = new MainMenu();
            Master = mainMenu;
            Detail = GetOrCreateDetail(typeof(HomePage));

            mainMenu.ListView.ItemSelected += OnItemSelected;
        }

        Page GetOrCreateDetail(Type pageType)
        {
            Page page;

            var pageExists = detailCache.TryGetValue(pageType, out page);

            if (!pageExists)
            {
                page = new CustomNavigationPage((Page)Activator.CreateInstance(pageType));
                detailCache.Add(pageType, page);
            }

            return page;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuItem;
            if (item != null && item.TargetType != null)
            {
                Detail = GetOrCreateDetail(item.TargetType);
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
