/// <reference path='../../typings/browser.d.ts'/>

module SkiResort {
    'use strict';

    export module App {
        angular.module('app', [
            'ionic',
            'app.core',
            'app.auth',
            'app.home',
            'app.lift',
            'app.dining',
            'app.rental'
        ]);
    }
}
