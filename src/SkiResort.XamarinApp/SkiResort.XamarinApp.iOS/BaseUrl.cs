using Xamarin.Forms;
using SkiResort.XamarinApp.iOS;
using Foundation;
using SkiResort.XamarinApp.Interfaces;

[assembly: Dependency (typeof (BaseUrl_iOS))]
namespace SkiResort.XamarinApp.iOS
{
    public class BaseUrl_iOS : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}
