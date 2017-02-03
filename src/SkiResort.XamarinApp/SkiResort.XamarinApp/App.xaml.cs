using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using SkiResort.XamarinApp.Pages;
using Xamarin.Forms.Xaml;
using FFImageLoading;
using SkiResort.XamarinApp.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SkiResort.XamarinApp
{
    public partial class App : Application
    {
        public static Page RootPage
        {
            get
            {
                return NavigationService.Instance.MasterDetailPage;
            }
            set { }
        }
        public App()
        {
            InitializeComponent();
            MainPage = App.RootPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
