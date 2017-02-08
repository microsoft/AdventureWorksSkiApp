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
            applyBarColorBasedOnPage(root);
        }

        private void applyBarColorBasedOnPage(Page page)
        {
            BarBackgroundColor = Config.DEFAULT_BAR_COLOR;
            BarTextColor = Config.DEFAULT_BAR_TEXT_COLOR;
            if (page is IBarTint)
            {
                var barTint = (IBarTint)page;

                BarBackgroundColor = barTint.GetBarBackgroundColor();
                BarTextColor = barTint.GetBarTextColor();
            }
        }
    }
}
