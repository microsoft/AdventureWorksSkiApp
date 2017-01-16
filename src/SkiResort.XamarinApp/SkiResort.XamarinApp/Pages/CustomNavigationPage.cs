using SkiResort.XamarinApp.Interfaces;
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
        public CustomNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = Color.FromHex("#141414");
            BarTextColor = Color.FromHex("#FFFFFF");
            if (root is IBarTint)
            {
                var barTint = (IBarTint)root;
                BarBackgroundColor = barTint.GetBarBackgroundColor();
                BarTextColor = barTint.GetBarTextColor();
            }
        }
    }
}
