using FFImageLoading.Forms;
using SkiResort.XamarinApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Views
{

    public class PriceLevelView : ContentView
    {
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(PriceLevel), typeof(PriceLevelView), PriceLevel.Unknown, propertyChanged: OnPropertiesChanged);
        public PriceLevel Value
        {
            get { return (PriceLevel)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        static void OnPropertiesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((PriceLevelView)bindable).OnPropertiesChanged();
        }
        protected virtual void OnPropertiesChanged()
        {
            createDollars();
        }

        private StackLayout layout;

        private int activeDollars {
            get
            {
                return (int)Value;
            }
        }

        public PriceLevelView()
        {
            layout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 1
            };
            Content = layout;

            createDollars();
        }

        private void createDollars()
        {
            layout.Children.Clear();
            for (var i = 0; i < 3; i++)
            {
                layout.Children.Add(createDollar(i < activeDollars));
            }
        }

        private Label createDollar(bool active)
        {
            return new Label
            {
                Text = "$",
                TextColor = active ? Color.FromHex("#1A90C9") : Color.Gray
            };
        }
    }
}
