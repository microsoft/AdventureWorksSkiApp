using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Pages
{
    public class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage(Page root, CustomNavigationPageStyle style) : base(root)
        {
            BarBackgroundColor = GetStyleBarColor(style);
            BarTextColor = Color.FromHex("#FFFFFF");
        }

        Color GetStyleBarColor(CustomNavigationPageStyle style)
        {
            if (style == CustomNavigationPageStyle.Black)
                return Color.FromHex("#141414");

            return Color.FromHex("#15719E");
        }
    }

    public enum CustomNavigationPageStyle
    {
        Black,
        Blue
    }
}
