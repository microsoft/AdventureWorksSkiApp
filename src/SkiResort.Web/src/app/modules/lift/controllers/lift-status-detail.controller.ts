/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Lift.Controllers {
    'use strict';

    class LiftStatusDetailController {

        private loading: boolean = false;
        private userGeolocation: Core.Services.IGeolocation = null;
        private distance: number = null;
        private lift: Models.Lift;

        constructor(
            private liftService: Services.LiftService,
            private $stateParams,
            private geoService: Core.Services.GeoService,
            private $scope
        ) {
            this.getLift();

            $scope.$watch(() => {
                this.getDistance();
            });
        }

        public getLift() {
            this.loading = true;
            this.liftService.get(this.$stateParams.liftId)
                .then((lift) => {
                    this.lift = lift;
                })
                .catch(() => {
                    // Handle error
                })
                .finally(() => {
                    this.loading = false;
                });
        }

        public getDistance() {
            if (this.userGeolocation) {
                this.calcDistance();
            } else {
                this.geoService.getPosition()
                    .then((data) => {
                        this.userGeolocation = data;
                        this.calcDistance();
                    });
            }
        }

        public calcDistance() {
            if (this.lift) {
                this.distance = this.geoService.getDistanceBetween(
                    this.userGeolocation,
                    {latitude: this.lift.latitude, longitude: this.lift.longitude});
            }
        }
    }

    angular.module('app.lift')
        .controller('LiftStatusDetailController', LiftStatusDetailController);
}
