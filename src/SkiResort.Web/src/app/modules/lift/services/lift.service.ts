/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Lift.Services {
    'use strict';

    export class LiftService {
        constructor(
            private $q,
            private liftAPI: LiftAPI,
            private geoService: Core.Services.GeoService
        ) {}

        public getAll(): ng.IPromise<Array<Models.Lift>> {
            return this.$q((resolve, reject) => {

                this.liftAPI.getAll()
                    .then((response) => {
                        resolve(response.data.map((item) => {
                            return new Models.Lift().serialize(item);
                        }));
                    });

            });
        }

        public getNear(): ng.IPromise<Array<Models.Lift>> {
            return this.$q((resolve, reject) => {

                this.geoService.getPosition()
                    .then((result) => {
                        return this.liftAPI.getNear(result.latitude, result.longitude);
                    })
                    .then((response) => {
                        resolve(response.data.map((item) => {
                            return new Models.Lift().serialize(item);
                        }));
                    })
                    .catch(reject);

            });
        }

        public get(id: string): ng.IPromise<Models.Lift> {
            return this.$q((resolve, reject) => {

                this.liftAPI.get(id)
                    .then((result) => {
                        resolve(new Models.Lift().serialize(result.data));
                    })
                    .catch(reject);

            });
        }
    }

    angular.module('app.lift')
        .service('liftService', LiftService);
}
