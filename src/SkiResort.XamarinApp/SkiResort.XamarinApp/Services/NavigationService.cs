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

        public NavigationService()
        {
            registerViewModels();
            MasterDetailPage = new MasterDetailPage();
            MasterDetailPage.Master = CreatePage(typeof(MainMenuViewModel));
            MasterDetailPage.Detail = new CustomNavigationPage(CreatePage(homeViewModel));

            MessagingCenter.Subscribe<MainMenuViewModel, MainMenuViewModel.MainMenuItem>(this, "MainMenuItemSelected", onMenuItemSelected);
            MessagingCenter.Subscribe<MainMenuViewModel>(this, "LogoutButtonClicked", onLogoutButtonClicked);
        }

        public async Task NavigateTo(Type viewModelType, params object[] parameters)
        {
            var navigationPage = MasterDetailPage.Detail as CustomNavigationPage;
            await navigationPage.Navigation.PushAsync(CreatePage(viewModelType, parameters), true);
        }

        public async Task NavigateBack()
        {
            var navigationPage = MasterDetailPage.Detail as CustomNavigationPage;
            await navigationPage.Navigation.PopAsync();
        }

        private void registerViewModels()
        {
            viewModelPageMapping = new Dictionary<Type, Type>();

            homeViewModel = typeof(HomeViewModel);

            viewModelPageMapping.Add(typeof(HomeViewModel), typeof(HomePage));
            viewModelPageMapping.Add(typeof(LoginViewModel), typeof(LoginPage));
            viewModelPageMapping.Add(typeof(LiftStatusViewModel), typeof(LiftStatusPage));
            viewModelPageMapping.Add(typeof(RentalViewModel), typeof(RentalPage));
            viewModelPageMapping.Add(typeof(RentalListViewModel), typeof(RentalListPage));
            viewModelPageMapping.Add(typeof(RentalFormViewModel), typeof(RentalFormPage));
            viewModelPageMapping.Add(typeof(DiningViewModel), typeof(DiningPage));
            viewModelPageMapping.Add(typeof(ReportViewModel), typeof(ReportPage));

            viewModelPageMapping.Add(typeof(LiftDetailViewModel), typeof(LiftDetailPage));
            viewModelPageMapping.Add(typeof(DiningDetailViewModel), typeof(DiningDetailPage));

            viewModelPageMapping.Add(typeof(MainMenuViewModel), typeof(MainMenu));
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

        private void onMenuItemSelected(MainMenuViewModel sender, MainMenuViewModel.MainMenuItem item)
        {
            if (item != null && item.TargetType != null)
            {
                MasterDetailPage.Detail = new CustomNavigationPage(CreatePage(item.TargetType));
            }
            MasterDetailPage.IsPresented = false;
        }

        private void onLogoutButtonClicked(MainMenuViewModel sender)
        {
            MasterDetailPage.IsPresented = false;
        }
    }
}
