using SkiResort.XamarinApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp
{
    public static class Config
    {
        public const string API_URL = "__SERVERURI__";
        public const double USER_DEFAULT_POSITION_LATITUDE = 40.7201013;
        public const double USER_DEFAULT_POSITION_LONGITUDE = -74.0101931;
        public static Color BAR_COLOR_BLACK = Color.FromHex("#141414");
    }
}
