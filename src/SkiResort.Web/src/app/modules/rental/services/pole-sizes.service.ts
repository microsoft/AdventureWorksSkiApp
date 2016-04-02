/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Rental.Services {
    'use strict';

    export function PoleSizes() {
        var result = [];
        for (var i = 32; i <= 57; i++) {
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
        .service('poleSizes', PoleSizes);
}
