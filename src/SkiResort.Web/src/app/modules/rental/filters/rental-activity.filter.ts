/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Filters {
    'use strict';

    function RentalActivityFilter () {
        return function(input: number): string {
            return Models.RentalActivity[input];
        };
    }

    angular.module('app.rental')
        .filter('skiRentalActivity', RentalActivityFilter);
}
