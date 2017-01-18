using SkiResort.XamarinApp.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp.Converters
{
    class LiftToAltitudeFormattedText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var lift = value as Lift;
            var formattedText = new FormattedString();
            formattedText.Spans.Add(new Span { Text = lift.Name, FontSize = 13 });
            formattedText.Spans.Add(new Span { Text = " 300ft", FontAttributes = FontAttributes.Bold, FontSize = 13 });
            return formattedText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
