using SkiResort.XamarinApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp.Converters
{
    class UserToWelcomeMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user = value as User;
            if (user != null)
            {
                var formattedText = new FormattedString();
                formattedText.Spans.Add(new Span { Text = "Welcome, ", FontSize = 19 });
                formattedText.Spans.Add(new Span { Text = user.FullName, FontAttributes = FontAttributes.Bold, FontSize = 19 });
                formattedText.Spans.Add(new Span { Text = "!", FontSize = 19 });
                return formattedText;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
