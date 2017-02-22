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
        public const string API_URL = "http://websiteadv6jz37bruvu5u6.azurewebsites.net";
        public const double USER_DEFAULT_POSITION_LATITUDE = 40.7201013;
        public const double USER_DEFAULT_POSITION_LONGITUDE = -74.0101931;

        public static string POWERBI_ACCESS_TOKEN = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2ZXIiOiIwLjIuMCIsImF1ZCI6Imh0dHBzOi8vYW5hbHlzaXMud2luZG93cy5uZXQvcG93ZXJiaS9hcGkiLCJpc3MiOiJQb3dlciBCSSBOb2RlIFNESyIsIndjbiI6InNraXJlc29ydCIsIndpZCI6IjEzZGNhMTUyLWRhMjMtNGYyYy1iMTRhLWQwYTg1YjdiMjEzYiIsInJpZCI6IjJjMTNiY2Q3LWQxYTgtNDg3Yy04NGVlLWYxZDhjYjUwMjA4ZCIsIm5iZiI6MTQ4Nzc3MzIxMSwiZXhwIjoxNDg3Nzc2ODExfQ.aDHeekUxJxOT_vwJSfu5QwE1IrNGos7TXKyFFBNNfZM";
        public static string POWERBI_REPORT_ID = "2c13bcd7-d1a8-487c-84ee-f1d8cb50208d";

        public static Color PRIMARY_COLOR = Color.FromHex("#1A90C9");
        public static Color DEFAULT_BAR_COLOR = Color.FromHex("#141414");
        public static Color DEFAULT_BAR_TEXT_COLOR = Color.FromHex("#FFFFFF");
        public static Color BLUE_BAR_COLOR = Color.FromHex("#15719E");
    }
}
