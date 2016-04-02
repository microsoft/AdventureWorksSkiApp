/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Services {
    'use strict';

    export class RentalAPI {
        constructor(
            private $q,
            private $http,
            private configService: Core.Services.ConfigService
        ) {}

        public get(rentalId) {
            return this.$q((resolve, reject) => {

                var request = {
                    url: `${this.configService.API.URL + this.configService.API.Path}rentals/${rentalId}`,
                    method: 'GET'
                };

                this.$http(request)
                    .then((response) => {
                        resolve(response);
                    });
            });
        }

        public getAll() {
            return this.$q((resolve, reject) => {

                var request = {
                    url: `${this.configService.API.URL + this.configService.API.Path}rentals`,
                    method: 'GET'
                };

                this.$http(request)
                    .then((response) => {
                        resolve(response);
                    });

            });
        }

        public add(rental) {
            var request = {
                url: `${this.configService.API.URL + this.configService.API.Path}rentals`,
                method: 'POST',
                data: rental
            };
            return this.$http(request);
        }

        public save(rental) {
            var request = {
                url: `${this.configService.API.URL + this.configService.API.Path}rentals`,
                method: 'PUT',
                data: rental
            };
            return this.$http(request);
        }

        public checkHighDemand(date: string) {
            var request = {
                url: `${this.configService.API.URL + this.configService.API.Path}rentals/check_high_demand?date=${date}`,
                method: 'GET'
            };
            return this.$http(request);
        }
    }

    angular.module('app.rental')
        .service('rentalAPI', RentalAPI);
}
