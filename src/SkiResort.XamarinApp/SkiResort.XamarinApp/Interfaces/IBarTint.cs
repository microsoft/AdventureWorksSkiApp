using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp.Interfaces
{
    interface IBarTint
    {
        Color GetBarBackgroundColor();
        Color GetBarTextColor();
    }
}
