/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Filters {
    'use strict';

    function RentalCategoryFilter () {
        return function(input: number): string {
            return Models.RentalCategory[input];
        };
    }

    angular.module('app.rental')
        .filter('skiRentalCategory', RentalCategoryFilter);
}
