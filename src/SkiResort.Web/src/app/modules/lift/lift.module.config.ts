/// <reference path='../../../../typings/browser.d.ts'/>

module SkiResort.App.Lift {
    'use strict';

    function routingConfiguration($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider) {
        $stateProvider
            .state('app.lift-status-list', {
                url: '/lift/status/list',
                views: {
                    'content': {
                        controller: 'LiftStatusListController',
                        controllerAs: '$ctrl',
                        templateUrl: 'lift/templates/lift-status-list.template.html',
                    }
                }
            })
            .state('app.lift-status-detail', {
                url: '/lift/status/detail/:liftId',
                views: {
                    'content': {
                        controller: 'LiftStatusDetailController',
                        controllerAs: '$ctrl',
                        templateUrl: 'lift/templates/lift-status-detail.template.html'
                    }
                }
            });
    }

    angular.module('app.lift')
        .config(routingConfiguration);
}
