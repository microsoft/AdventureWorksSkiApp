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

        public static string POWERBI_ACCESS_TOKEN = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ2ZXIiOiIwLjIuMCIsIndjbiI6InNraXJlc29ydCIsIndpZCI6IjNjOWQ2MjU0LTMwY2MtNGRlZi04MmQ2LTk2OTUwNDI5ZTU1MyIsInJpZCI6ImJjY2Q2OGE4LTI5MDctNDRhMi1iNjZhLWMyMmE0Y2UzZTA1MSIsImlzcyI6IlBvd2VyQklTREsiLCJhdWQiOiJodHRwczovL2FuYWx5c2lzLndpbmRvd3MubmV0L3Bvd2VyYmkvYXBpIiwiZXhwIjoxNDg2NzI4MjA4LCJuYmYiOjE0ODY3MjQ2MDh9.tMUr8KsxmZEdl6naQDUSUCxPlQHSlElK-WemSDdvEIk";
        public static string POWERBI_REPORT_ID = "bccd68a8-2907-44a2-b66a-c22a4ce3e051";

        public static Color PRIMARY_COLOR = Color.FromHex("#1A90C9");
        public static Color DEFAULT_BAR_COLOR = Color.FromHex("#141414");
        public static Color DEFAULT_BAR_TEXT_COLOR = Color.FromHex("#FFFFFF");
        public static Color BLUE_BAR_COLOR = Color.FromHex("#15719E");
    }
}
