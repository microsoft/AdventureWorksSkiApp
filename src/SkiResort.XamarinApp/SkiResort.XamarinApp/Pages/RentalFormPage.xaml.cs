using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Pages
{
    public partial class RentalFormPage : ContentPage
    {
        public RentalFormPage()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Icon = "plus-square-o-menuicon.png";
            }
            InitializeComponent();
        }
    }
}
