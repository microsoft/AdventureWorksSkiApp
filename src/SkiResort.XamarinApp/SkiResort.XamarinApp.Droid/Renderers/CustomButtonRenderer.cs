using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Views;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using SkiResort.XamarinApp.Views;
using SkiResort.XamarinApp.Droid.Renderers;
using Android.Graphics.Drawables;
using Android.Content.Res;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace SkiResort.XamarinApp.Droid.Renderers
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            var element = this.Element as Button;

            if (element == null || this.Control == null)
            {
                return;
            }

            this.Control.Gravity = GravityFlags.CenterVertical | GravityFlags.Left;
            this.Control.SetPadding(0, 0, 0, 0);
            this.Control.Clickable = true;
        }
    }
}