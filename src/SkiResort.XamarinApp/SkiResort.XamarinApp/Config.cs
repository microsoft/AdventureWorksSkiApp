﻿using SkiResort.XamarinApp.Entities;
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

        public static string POWERBI_ACCESS_TOKEN = "__ACCESSTOKEN__";
        public static string POWERBI_REPORT_ID = "__REPORTID__";

        public static Color PRIMARY_COLOR = Color.FromHex("#1A90C9");
        public static Color DEFAULT_BAR_COLOR = Color.FromHex("#141414");
        public static Color DEFAULT_BAR_TEXT_COLOR = Color.FromHex("#FFFFFF");
        public static Color BLUE_BAR_COLOR = Color.FromHex("#15719E");
    }
}
