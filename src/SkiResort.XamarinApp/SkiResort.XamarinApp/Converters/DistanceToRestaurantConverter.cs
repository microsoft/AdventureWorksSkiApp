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
    class DistanceToRestaurantConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var restaurant = (Restaurant)value;
            if (restaurant != null)
            {
                var aCoord = new Geolocation {
                    Latitude = restaurant.Latitude,
                    Longitude = restaurant.Longitude
                };
                var bCoord = new Geolocation {
                    Latitude = Config.USER_DEFAULT_POSITION_LATITUDE,
                    Longitude = Config.USER_DEFAULT_POSITION_LONGITUDE
                };
                var distance = distanceBetween(aCoord, bCoord);
                return distance;
            }
            return 0;
        }

        double distanceBetween(Geolocation aCoord, Geolocation bCoord)
        {
            var baseRad = Math.PI * aCoord.Latitude / 180;
            var targetRad = Math.PI * bCoord.Latitude / 180;
            var theta = aCoord.Longitude - bCoord.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
