using SkiResort.XamarinApp.Droid;
using SkiResort.XamarinApp.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_Android))]
namespace SkiResort.XamarinApp.Droid
{
    public class BaseUrl_Android : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
    }
}
