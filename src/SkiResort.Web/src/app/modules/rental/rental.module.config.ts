/// <reference path='../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental {
    'use strict';

    function routingConfiguration($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider) {
        $stateProvider
            .state('app.rentals', {
                cache: false,
                url: '/rentals',
                views: {
                    'content': {
                        templateUrl: 'rental/templates/rentals.template.html',
                        controller: 'RentalsController',
                        controllerAs: '$ctrl'
                    }
                }
            })
            .state('app.rental-detail', {
                cache: false,
                url: '/rental/:rentalId',
                views: {
                    'content': {
                        templateUrl: 'rental/templates/new-reservation-wrapper.template.html',
                        controller: 'NewReservationController',
                        controllerAs: 'tabCtrl'
                    }
                }
            });
    }

    angular.module('app.rental')
        .config(routingConfiguration);
}
