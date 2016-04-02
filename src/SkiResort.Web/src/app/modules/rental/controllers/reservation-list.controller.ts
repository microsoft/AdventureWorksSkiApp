/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Controllers {
    'use strict';

    class ReservationListController {
        private rentals: Array<Models.Rental>;
        private loading: boolean;
        private showMessage: boolean = false;
        private navigatingRental: Models.Rental;

        constructor(
            private $scope,
            private rentalService: Services.RentalService,
            private $state: ng.ui.IStateService,
            private $timeout: ng.ITimeoutService
        ) {
            this.getRentals();

            $scope.$on('update_reservation_list', () => {
                this.getRentals()
                    .finally(() => {
                        this.showMessage = true;
                    });
            });
        }

        public getRentals() {
            this.loading = true;
            return this.rentalService.getAll()
                .then((data) => {
                    this.rentals = data;
                })
                .finally(() => {
                    this.loading = false;
                });
        }

        public navigateToRental(rental: Models.Rental) {
            this.navigatingRental = rental;
            this.$timeout(() => {
                this.$state.go('app.rental-detail', {rentalId: rental.rentalId});
            }, 100);
        }
    }

    angular.module('app.rental')
        .controller('ReservationListController', ReservationListController);
}
