/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Services {
    'use strict';

    export function SkiSizes() {
        var result = [];
        for (var i = 115; i <= 200; i++) {
            result.push({
                id: i,
                label: i + ' in'
            });
        };

        return {
            getById: (id) => ({id: id, label: id + ' in'}),
            all: result
        };
    }

    angular.module('app.rental')
        .service('skiSizes', SkiSizes);
}
