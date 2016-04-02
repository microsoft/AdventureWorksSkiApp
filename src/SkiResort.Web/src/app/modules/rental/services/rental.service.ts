/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Services {
    'use strict';

    export class RentalService {
        constructor(
            private $q,
            private rentalAPI: RentalAPI,
            private geoService: Core.Services.GeoService
        ) {}

        public get(rentalId): ng.IPromise<Models.Rental> {
            return this.$q((resolve, reject) => {

                this.rentalAPI.get(rentalId)
                    .then((response) => {
                        resolve(new Models.Rental().serialize(response.data));
                    });
            });
        }

        public getAll(): ng.IPromise<Array<Models.Rental>> {
            return this.$q((resolve, reject) => {

                this.rentalAPI.getAll()
                    .then((response) => {
                        resolve(response.data.map((item) => {
                            return new Models.Rental().serialize(item);
                        }));
                    });

            });
        }

        public add(rental: Models.Rental) {
            return this.$q((resolve, reject) => {

                this.rentalAPI.add(rental.deserialize())
                    .then(() => {
                        resolve();
                    })
                    .catch(() => {
                        reject();
                    });

            });
        }

        public save(rental: Models.Rental) {
            return this.$q((resolve, reject) => {

                this.rentalAPI.save(rental.deserialize())
                    .then(() => {
                        resolve();
                    })
                    .catch(() => {
                        reject();
                    });

            });
        }

        public checkHighDemand(date: Date) {
            return this.$q((resolve, reject) => {

                this.rentalAPI.checkHighDemand(date.toISOString())
                    .then((response) => {
                        resolve(response.data);
                    })
                    .catch(() => {
                        reject();
                    });

            });
        }

    }

    angular.module('app.rental')
        .service('rentalService', RentalService);
}
