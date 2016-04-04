/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Controllers {
    'use strict';

    class NewReservationController {

        private loading: boolean = false;
        private highDemandAlert: boolean = false;
        private rental: Models.Rental;
        private rentalForm: ng.IFormController;

        constructor(
            private $scope,
            private rentalService: Services.RentalService,
            private rentalActivities,
            private rentalCategories,
            private rentalGoals,
            private shoeSizes,
            private skiSizes,
            private poleSizes,
            private $stateParams,
            private pickupHours: Services.PickupHours,
            private navigationService: Core.Services.NavigationService
        ) {
            // Initialize rental
            this.rental = new Models.Rental();
            this.pickupHours.generateAll();

            // If there's a rentalId, we're editing, not creating
            if ($stateParams.rentalId) {
                this.getRental();
            } else {
                this.resetRental();
            }

            // Set the todayDated, used as 'min' value for startDate
            $scope.todayDate = new Date();
            $scope.todayDate.setHours(0);
            $scope.todayDate.setMinutes(0);
            $scope.todayDate.setSeconds(0);

            /*
                startDate is edited in two separated inputs,
                so, it needs to be in reparated models for
                each ng-model
            */
            $scope.startDate = {
                day: null,
                hour: null
            };

            /*
                These values (options in selectors) are mapped to the original
                rental model using $scope.$watchCollection
            */
            $scope.rentalTemp = {
                activity: null,
                category: null,
                poleSize: null,
                skiSize: null
            };

            // Update startDate
            $scope.$watchCollection('startDate', () => {
                if (!$scope.startDate.day) { return; }
                this.rental.startDate = angular.copy($scope.startDate.day);
                if ($scope.startDate.hour) {
                    this.rental.startDate.setHours($scope.startDate.hour.hours);
                    this.rental.startDate.setMinutes($scope.startDate.hour.minutes);
                }
                this.checkHighDemand();
            });

            // Update activity, category, poleSize and skiSize
            $scope.$watchCollection('rentalTemp', () => {
                this.rental.activity = $scope.rentalTemp.activity ? $scope.rentalTemp.activity.id : null;
                this.rental.category = $scope.rentalTemp.category ? $scope.rentalTemp.category.id : null;
                this.rental.poleSize = $scope.rentalTemp.poleSize ? $scope.rentalTemp.poleSize.id : null;
                this.rental.skiSize = $scope.rentalTemp.skiSize ? $scope.rentalTemp.skiSize.id : null;
            });
        }

        public getRental() {
            this.loading = true;
            this.rentalService.get(this.$stateParams.rentalId)
                .then((rental) => {
                    this.rental = rental;
                    this.updateTempData();
                    this.loading = false;
                });
        }

        // This will update all the temporal data used in the custom selects
        // based on the actual rental object.
        //
        // This method is used at start when we're editing a rental
        public updateTempData() {
            var hours = this.rental.startDate.getHours();
            var minutes = this.rental.startDate.getMinutes();

            this.$scope.startDate = {
                day: this.rental.startDate,
                hour: this.pickupHours.getById((hours * 60) + minutes)
            };

            this.$scope.rentalTemp = {
                activity: this.rentalActivities.getById(this.rental.activity),
                category: this.rentalCategories.getById(this.rental.category),
                poleSize: this.poleSizes.getById(this.rental.poleSize),
                skiSize: this.skiSizes.getById(this.rental.skiSize)
            };
        }

        // This will put the rental and all the temporal values for custom
        // selects to null
        //
        // This method is used at start when we're creating a rental
        public resetRental() {
            this.rental = new Models.Rental();

            for (var i in this.$scope.startDate) {
                if (this.$scope.startDate.hasOwnProperty(i)) {
                    this.$scope.startDate[i] = null;
                }
            }

            for (var x in this.$scope.rentalTemp) {
                if (this.$scope.rentalTemp.hasOwnProperty(x)) {
                    this.$scope.rentalTemp[x] = null;
                }
            }
        }

        public emitSave() {
            this.$scope.$emit('rental_saved');
        }

        public submitReservation() {
            if (this.rentalForm.$valid) {
                this.loading = true;
                var actionPromise = null;
                if (this.$stateParams.rentalId) {
                    actionPromise = this.saveReservation();
                } else {
                    actionPromise = this.addReservation();
                }
                actionPromise
                    .finally(() => {
                        this.loading = false;
                    });
            }
        }

        public addReservation() {
            return this.rentalService.add(this.rental)
                .then(() => {
                    this.resetRental();
                    this.rentalForm.$setPristine();
                    this.rentalForm.$setUntouched();
                    this.emitSave();
                });
        }

        public saveReservation() {
            return this.rentalService.save(this.rental)
                .then(() => {
                    this.navigationService.goBack();
                });
        }

        public checkHighDemand() {
            this.rentalService.checkHighDemand(this.rental.startDate)
                .then((result) => {
                    this.highDemandAlert = result;
                })
                .catch(() => {
                    // Handle error
                });
        }

        public getCalculatedCost() {
            this.rental.totalCost = 0;

            if (this.rental.startDate && this.rental.endDate) {
                this.rental.totalCost = 20;
                var days = Math.floor((this.rental.endDate.getTime() - this.rental.startDate.getTime()) / 1000 / 60 / 60 / 24);
                this.rental.totalCost = this.rental.totalCost + (days * 5);
            }

            return this.rental.totalCost;
        }
    }

    angular.module('app.rental')
        .controller('NewReservationController', NewReservationController);
}
