/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Lift.Controllers {
    'use strict';

    class LiftStatusListController {

        private openLifts: Array<Models.Lift>;
        private closedLifts: Array<Models.Lift>;
        private loading: boolean;

        constructor(private liftService: Services.LiftService) {
            this.getLifts();
        }

        public getLifts() {
            this.loading = true;
            this.liftService.getNear()
                .then((data) => {
                    this.digestLifts(data);
                })
                .finally(() => {
                    this.loading = false;
                });
        }

        public digestLifts(lifts: Array<Models.Lift>) {
            this.openLifts = [];
            this.closedLifts = [];

            lifts.forEach((lift) => {
                if (lift.status === Models.LiftStatus.Open) {
                    this.openLifts.push(lift);
                } else if (lift.status === Models.LiftStatus.Closed) {
                    this.closedLifts.push(lift);
                }
            });
        }
    }

    angular.module('app.lift')
        .controller('LiftStatusListController', LiftStatusListController);
}
