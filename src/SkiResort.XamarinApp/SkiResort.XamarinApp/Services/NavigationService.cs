using SkiResort.XamarinApp.Pages;
using SkiResort.XamarinApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp.Services
{
    class NavigationService
    {
        #region Singleton
        private static NavigationService instance;
        public static NavigationService Instance
        {
            get {
                if (instance == null)
                    instance = new NavigationService();
                return instance;
            }
        }
        #endregion

        public MasterDetailPage MasterDetailPage;
        public Page MainPage;

        private Dictionary<Type, Type> viewModelPageMapping;
        private Type homeViewModel;
        private MainMenu mainMenu;

        public NavigationService()
        {
            registerViewModels();
            mainMenu = new MainMenu();
            MasterDetailPage = new MasterDetailPage();
            MasterDetailPage.Master = mainMenu;
            MasterDetailPage.Detail = new CustomNavigationPage(createPage(homeViewModel));

            mainMenu.ListView.ItemSelected += OnMenuItemSelected;
        }

        public void NavigateTo(Type viewModelType)
        {
            var navigationPage = MasterDetailPage.Detail as CustomNavigationPage;
            navigationPage.PushAsync(createPage(viewModelType));
        }

        private void registerViewModels()
        {
            viewModelPageMapping = new Dictionary<Type, Type>();

            homeViewModel = typeof(HomeViewModel);

            viewModelPageMapping.Add(typeof(HomeViewModel), typeof(HomePage));
            viewModelPageMapping.Add(typeof(LiftStatusViewModel), typeof(LiftStatusPage));
            viewModelPageMapping.Add(typeof(RentalViewModel), typeof(RentalPage));
            viewModelPageMapping.Add(typeof(DiningViewModel), typeof(DiningPage));

            viewModelPageMapping.Add(typeof(LiftDetailViewModel), typeof(LiftDetailPage));
        }

        Page createPage(Type viewModelType)
        {
            Page page;
            Type pageType;
            var viewModelExists = viewModelPageMapping.TryGetValue(viewModelType, out pageType);

            if (!viewModelExists)
            {
                throw new Exception("ViewModel Type not found");
            }

            page = (Page)Activator.CreateInstance(pageType);
            page.BindingContext = Activator.CreateInstance(viewModelType);

            return page;
        }

        void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuItem;
            if (item != null && item.TargetType != null)
            {
                MasterDetailPage.Detail = new CustomNavigationPage(createPage(item.TargetType));
            }
            CloseMenu();
        }

        void CloseMenu()
        {
            (mainMenu).ListView.SelectedItem = null;
            MasterDetailPage.IsPresented = false;
        }
    }
}
