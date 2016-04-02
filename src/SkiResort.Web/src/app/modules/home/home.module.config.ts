/// <reference path='../../../../typings/browser.d.ts'/>

module SkiResort.App.Home {
    'use strict';

    function routingConfiguration($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider) {
        $stateProvider
            .state('app.home', {
                url: '/home',
                data: {
                    barColor: 'dark'
                },
                views: {
                    'content': {
                        controller: 'HomeController',
                        controllerAs: '$ctrl',
                        templateUrl: 'home/templates/home.template.html',
                    }
                }
            });

        $urlRouterProvider.otherwise('/home');
    }

    angular.module('app.home')
        .config(routingConfiguration);
}
