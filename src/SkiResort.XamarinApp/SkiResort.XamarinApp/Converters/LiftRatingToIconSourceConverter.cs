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
    class LiftRatingToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string iconSource = "";
            if (targetType == typeof(ImageSource))
            {
                iconSource = liftRatingToString((LiftRating)value);
            }
            return iconSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        string liftRatingToString(LiftRating liftRating)
        {
            string result = "";
            switch (liftRating)
            {
                case LiftRating.Beginner:
                    result = "liftStatus_circle.png";
                    break;
                case LiftRating.Intermediate:
                    result = "liftStatus_square.png";
                    break;
                case LiftRating.Advanced:
                    result = "liftStatus_diamond.png";
                    break;
            }
            return result;
        }
    }
}
