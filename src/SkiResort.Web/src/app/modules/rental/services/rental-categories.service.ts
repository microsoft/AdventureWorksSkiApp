/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Services {
    'use strict';

    export function RentalCategories() {
        return {
            getById: (id) => ({ id: id, label: Models.RentalCategory[id] }),
            all: Object.keys(Models.RentalCategory)
                .map(i => parseInt(i, 10))
                .filter(i => !isNaN(i) && i !== 0)
                .map(i => ({ id: i, label: Models.RentalCategory[i] }))
        };
    }

    angular.module('app.rental')
        .service('rentalCategories', RentalCategories);
}
