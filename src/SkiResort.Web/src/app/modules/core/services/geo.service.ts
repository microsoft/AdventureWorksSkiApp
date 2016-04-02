/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Core.Services {
    'use strict';

    const PI180 = 0.017453292519943295; // (Math.PI/180)

    export interface IGeolocation {
        latitude: number;
        longitude: number;
    }

    export class GeoService {
        constructor(
            private $q,
            private configService: ConfigService,
            private $log: ng.ILogService
        ) {}

        private geolocationApiAvailable() {
            return (navigator.geolocation && navigator.geolocation.getCurrentPosition);
        }

        public getPosition(): ng.IPromise<IGeolocation> {
            return this.$q((resolve, reject) => {
                this.$log.info('Geolocation requested');
                if (this.configService.General.FakeGeolocation) {
                    this.$log.info('- Returning fake geolocation');
                    resolve(this.configService.General.Geolocation);
                } else if (this.geolocationApiAvailable()) {
                    navigator.geolocation.getCurrentPosition((result) => {
                       resolve({
                           latitude: result.coords.latitude,
                           longitude: result.coords.longitude
                       });
                    }, () => {
                        reject();
                    });
                } else {
                    this.$log.warn('- Geolocation API not available');
                    reject();
                }

            });
        }

        public getDistanceBetween(a: IGeolocation, b: IGeolocation) {
            var p = PI180;
            var c = Math.cos;
            var r = 0.5 - c((a.latitude - b.latitude) * p) / 2 + c(b.latitude * p) *
                    c(a.latitude * p) * (1 - c((a.longitude - b.longitude) * p)) / 2;
            var result = (12742 * Math.asin(Math.sqrt(r)) * 0.621371); // 2 * R; R = 6371 km
            return Math.round(result * 1e2) / 1e2;
        }
    }

    angular.module('app.core')
        .service('geoService', GeoService);
}
