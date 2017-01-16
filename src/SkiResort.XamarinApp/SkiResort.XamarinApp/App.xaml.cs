using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using SkiResort.XamarinApp.Pages;
using Xamarin.Forms.Xaml;
using FFImageLoading;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SkiResort.XamarinApp
{
    public partial class App : Application
    {
        public App()
        {
            ImageService.Instance.LoadUrl("star_0.png").Preload();
            ImageService.Instance.LoadUrl("star_1.png").Preload();
            ImageService.Instance.LoadUrl("star_2.png").Preload();
            ImageService.Instance.LoadUrl("star_3.png").Preload();
            ImageService.Instance.LoadUrl("star_4.png").Preload();
            ImageService.Instance.LoadUrl("star_5.png").Preload();

            InitializeComponent();
            MainPage = new MainMasterDetailPage();
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
