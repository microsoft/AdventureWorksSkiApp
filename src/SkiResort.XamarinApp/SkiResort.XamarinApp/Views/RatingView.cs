using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Views
{
    public class RatingView : ContentView
    {
        public static readonly BindableProperty MaxProperty = BindableProperty.Create("Max", typeof(int), typeof(RatingView), 5, propertyChanged: OnPropertiesChanged);
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(int), typeof(RatingView), 3, propertyChanged: OnPropertiesChanged);
        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }
        
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        static void OnPropertiesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((RatingView)bindable).OnPropertiesChanged();
        }
        protected virtual void OnPropertiesChanged()
        {
            createStars();
        }

        private StackLayout layout;

        public RatingView()
        {
            layout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 1
            };
            Content = layout;

            createStars();
        }

        private void createStars()
        {
            layout.Children.Clear();
            for (var i = 0; i < Max; i++)
            {
                layout.Children.Add(createStar(i < Value));
            }
        }

        private CachedImage createStar(bool active)
        {
            return new CachedImage
            {
                Source = active ? "star_filled.png" : "star.png",
                WidthRequest = 10,
                HeightRequest = 10,
                DownsampleToViewSize = true
            };
        }
    }
}
