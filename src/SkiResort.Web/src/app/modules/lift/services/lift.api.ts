/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Lift.Services {
    'use strict';

    export class LiftAPI {
        constructor(
            private $q,
            private $http,
            private configService: Core.Services.ConfigService
        ) {}

        public getAll() {
            return this.$q((resolve, reject) => {

                var request = {
                    url: `${this.configService.API.URL + this.configService.API.Path}lifts`,
                    method: 'GET'
                };

                this.$http(request)
                    .then((response) => {
                        resolve(response);
                    });

            });
        }

        public getNear(latitude: number, longitude: number) {
            return this.$q((resolve, reject) => {

                var request = {
                    url: `${this.configService.API.URL + this.configService.API.Path}lifts/nearby` +
                        `?latitude=${latitude}` +
                        `&longitude=${longitude}`,
                    method: 'GET'
                };

                this.$http(request)
                    .then(resolve, reject);

            });
        }

        public get(id: string) {
            var request = {
                url: `${this.configService.API.URL + this.configService.API.Path}lifts/${id}`,
                method: 'GET'
            };
            return this.$http(request);
        }
    }

    angular.module('app.lift')
        .service('liftAPI', LiftAPI);
}
