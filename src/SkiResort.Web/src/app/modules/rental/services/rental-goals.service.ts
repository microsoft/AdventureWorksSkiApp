/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Services {
    'use strict';

    export function RentalGoals() {
        return Object.keys(Models.RentalGoal)
                .map(i => parseInt(i, 10))
                .filter(i => !isNaN(i))
                .map(i => ({ id: i, label: Models.RentalGoal[i] }));
    }

    angular.module('app.rental')
        .service('rentalGoals', RentalGoals);
}
