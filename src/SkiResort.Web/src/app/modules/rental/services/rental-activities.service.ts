/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Services {
    'use strict';

    export function RentalActivities() {
        return {
            getById: (id) => ({ id: id, label: Models.RentalActivity[id] }),
            all: Object.keys(Models.RentalActivity)
                .map(i => parseInt(i, 10))
                .filter(i => !isNaN(i))
                .map(i => ({ id: i, label: Models.RentalActivity[i] }))
        };
    }

    angular.module('app.rental')
        .service('rentalActivities', RentalActivities);
}
