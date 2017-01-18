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

        private Dictionary<Type, Type> viewModelPageMapping;
        private Type homeViewModel;
        private MainMenu mainMenu;

        public NavigationService()
        {
            registerViewModels();
            mainMenu = new MainMenu();
            MasterDetailPage = new MasterDetailPage();
            MasterDetailPage.Master = mainMenu;
            MasterDetailPage.Detail = new CustomNavigationPage(CreatePage(homeViewModel));

            mainMenu.ListView.ItemSelected += OnMenuItemSelected;
        }

        public void NavigateTo(Type viewModelType, params object[] parameters)
        {
            var navigationPage = MasterDetailPage.Detail as CustomNavigationPage;
            navigationPage.PushAsync(CreatePage(viewModelType, parameters));
        }

        private void registerViewModels()
        {
            viewModelPageMapping = new Dictionary<Type, Type>();

            homeViewModel = typeof(HomeViewModel);

            viewModelPageMapping.Add(typeof(HomeViewModel), typeof(HomePage));
            viewModelPageMapping.Add(typeof(LiftStatusViewModel), typeof(LiftStatusPage));
            viewModelPageMapping.Add(typeof(RentalViewModel), typeof(RentalPage));
            viewModelPageMapping.Add(typeof(RentalListViewModel), typeof(RentalListPage));
            viewModelPageMapping.Add(typeof(RentalFormViewModel), typeof(RentalFormPage));
            viewModelPageMapping.Add(typeof(DiningViewModel), typeof(DiningPage));

            viewModelPageMapping.Add(typeof(LiftDetailViewModel), typeof(LiftDetailPage));
            viewModelPageMapping.Add(typeof(DiningDetailViewModel), typeof(DiningDetailPage));
        }

        public Page CreatePage(Type viewModelType, params object[] parameters)
        {
            Page page;
            Type pageType;
            var viewModelExists = viewModelPageMapping.TryGetValue(viewModelType, out pageType);

            if (!viewModelExists)
            {
                throw new Exception("ViewModel Type not found");
            }

            page = (Page)Activator.CreateInstance(pageType);
            page.BindingContext = Activator.CreateInstance(viewModelType, parameters);

            return page;
        }

        void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuItem;
            if (item != null && item.TargetType != null)
            {
                MasterDetailPage.Detail = new CustomNavigationPage(CreatePage(item.TargetType));
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
