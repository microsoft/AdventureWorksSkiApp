/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Controllers {
    'use strict';

    export class RentalsController {
        constructor(
            private $ionicTabsDelegate,
            private $scope,
            private $stateParams
        ) {
            $scope.$on('rental_saved', () => {
                $ionicTabsDelegate.$getByHandle('section-tabs').select(0);
                $scope.$broadcast('update_reservation_list');
            });
        }
    }

    angular.module('app.rental')
        .controller('RentalsController', RentalsController);
}
