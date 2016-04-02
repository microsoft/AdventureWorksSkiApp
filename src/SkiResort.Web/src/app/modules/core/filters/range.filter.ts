/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Filters {
    'use strict';

    function Range () {
        return function(input: Array<number>, _min: string, _max: string): Array<number> {
            var min: number = parseInt(_min, 10);
            var max: number = parseInt(_max, 10);
            for (var i = min; i < max; i++) {
                input.push(i);
            }
            return input;
        };
    }

    angular.module('app.rental')
        .filter('skiRange', Range);
}
