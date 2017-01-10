using SkiResort.XamarinApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Views
{
    public partial class RentalView : ContentView
    {
        public static readonly BindableProperty RentalProperty = BindableProperty.Create("Rental", typeof(Rental), typeof(RentalView), default(Rental));
        public Rental Rental
        {
            get { return (Rental)GetValue(RentalProperty); }
            set { SetValue(RentalProperty, value); }
        }
        public RentalView()
        {
            InitializeComponent();
        }
    }
}
