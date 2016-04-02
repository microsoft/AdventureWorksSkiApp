/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Dining.Services {
    'use strict';

    import Restaurant = Dining.Models.Restaurant;

    export class DiningAPI {

        constructor(
            private $q,
            private $http,
            private configService: Core.Services.ConfigService
        ) {}

        public getAll(): angular.IHttpPromise<Array<Restaurant>> {
            var request = {
                url: `${this.configService.API.URL + this.configService.API.Path}restaurants`,
                method: 'GET'
            };

            return this.$http(request).then(results => {
                return results.data;
            });
        }

        public getNear(latitude: number, longitude: number): angular.IHttpPromise<Array<Restaurant>> {
            var request = {
                url: `${this.configService.API.URL + this.configService.API.Path}restaurants/nearby` +
                `?latitude=${latitude}` +
                `&longitude=${longitude}`,
                method: 'GET'
            };

            return this.$http(request).then(results => {
                return results.data;
            });
        }

        public getRecommendations(searchtext: string): angular.IHttpPromise<Array<number>> {
            var request = {
                url: `${this.configService.API.URL + this.configService.API.Path}restaurants/recommendations/${searchtext}`,
                method: 'GET'
            };

            return this.$http(request).then(results => {
                return results.data;
            });
        }

        public getSingle(id: number): angular.IHttpPromise<Restaurant> {
            var request = {
                url: `${this.configService.API.URL + this.configService.API.Path}restaurants/${id}`,
                method: 'GET'
            };

            return this.$http(request).then(results => {
                return results.data;
            });
        }

        public getImage(id: number): any {
            return `${this.configService.API.URL + this.configService.API.Path}restaurants/photo/${id}`;
        }
    }

    angular.module('app.dining')
        .service('diningAPI', DiningAPI);
}
