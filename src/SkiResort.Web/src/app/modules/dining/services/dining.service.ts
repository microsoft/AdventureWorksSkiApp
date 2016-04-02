/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Dining.Services {
    'use strict';

    import Restaurant = Dining.Models.Restaurant;

    export class DiningService {
        private latitude: number = 0;
        private longitude: number = 0;

        constructor(
            private $q,
            private diningAPI: DiningAPI,
            private geoService: Core.Services.GeoService
        ) { /* */ }

        private getDistance(latitude, longitude) {
            return this.geoService.getDistanceBetween(
                {latitude: latitude, longitude: longitude},
                {latitude: this.latitude, longitude: this.longitude});
        }

        private calculateDistances(restaurants): Array<Restaurant> {
            for (var i = 0, l = restaurants.length; i < l; i++) {
                restaurants[i].distance = this.getDistance(restaurants[i].latitude, restaurants[i].longitude);
            }

            return restaurants;
        }

        public getAll(): ng.IPromise<Array<Restaurant>> {
            return this.diningAPI.getAll();
        }

        public getNear(): ng.IPromise<Array<Restaurant>> {
            return this.$q((resolve) => {
                return this.geoService.getPosition()
                        .then((result) => {
                            this.latitude = result.latitude;
                            this.longitude = result.longitude;
                            return this.diningAPI.getNear(this.latitude, this.longitude).then(restaurants => {
                                return resolve(this.calculateDistances(restaurants));
                            });
                        });
            });
        }

        public getRecommendations(_searchtext: string): ng.IPromise<Array<Restaurant>> {

            var searchtextSplitted = _searchtext.split(' ');
            var searchtext = searchtextSplitted[searchtextSplitted.length - 1];

            return this.$q((resolve) => {
                return this.diningAPI.getRecommendations(searchtext).then((ids: Array<number>) => {
                    var restaurants: Array<Restaurant> = [];
                    var promises = [];
                    for (var i = 0, l = ids.length; i < l; i++) {
                        promises.push((() => {
                            return this.getSingle(ids[i]).then((restaurant: Restaurant) => {
                                restaurants.push(restaurant);
                            });
                        })());
                    }

                    this.$q.all(promises).then(() => {
                        resolve(restaurants);
                    });
                });
            });
        }

        public getSingle(id: number): ng.IPromise<Restaurant> {
            return this.diningAPI.getSingle(id);
        }

        public getFilters(): Array<any> {
            return [
                { id: '-rating', label: 'Rating'},
                { id: 'levelOfNoise', label: 'Level Noise'},
                { id: 'priceLevel', label: 'Price'},
                { id: 'distance', label: 'Miles Away' },
                { id: '-familyFriendly', label: 'Family Friendly' }
            ];
        }

        public getImage(id: number): any {
            return this.diningAPI.getImage(id);
        }
    }

    angular.module('app.dining')
        .service('diningService', DiningService);
}
