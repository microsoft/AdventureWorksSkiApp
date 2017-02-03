using SkiResort.XamarinApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Views
{
    public partial class DiningView : ContentView
    {
        public static readonly BindableProperty RestaurantProperty = BindableProperty.Create("Restaurant", typeof(Restaurant), typeof(RentalView), default(Restaurant));
        public Restaurant Restaurant
        {
            get { return (Restaurant)GetValue(RestaurantProperty); }
            set { SetValue(RestaurantProperty, value); }
        }

        public DiningView()
        {
            InitializeComponent();
        }
    }
}
