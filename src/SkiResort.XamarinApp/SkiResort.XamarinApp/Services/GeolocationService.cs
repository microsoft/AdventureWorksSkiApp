using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiResort.XamarinApp.Entities;

namespace SkiResort.XamarinApp.Services
{
    class GeolocationService
    {
        public double DistanceTo(Geolocation coord) {
            var currentCoord = new Geolocation
            {
                Latitude = Config.USER_DEFAULT_POSITION_LATITUDE,
                Longitude = Config.USER_DEFAULT_POSITION_LONGITUDE
            };
            return DistanceBetween(coord, currentCoord);
        }

        public double DistanceBetween(Geolocation aCoord, Geolocation bCoord)
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
    }
}
