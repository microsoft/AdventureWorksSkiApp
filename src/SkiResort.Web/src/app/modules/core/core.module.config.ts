/// <reference path='../../../../typings/browser.d.ts'/>

module SkiResort.App.Core {
    'use strict';

    function routingConfiguration($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app', {
                url: '',
                abstract: true,
                controller: 'MenuController',
                controllerAs: '$ctrl',
                templateUrl: 'core/templates/menu.template.html'
            });
    }

    function ionicConfiguration($ionicConfigProvider) {
        $ionicConfigProvider.scrolling.jsScrolling(false);
    }

    angular.module('app.core')
        .config(routingConfiguration)
        .config(ionicConfiguration);
}
