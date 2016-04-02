/// <reference path='../../../../typings/browser.d.ts'/>

module SkiResort.App.Dining {
    'use strict';

    function routingConfiguration($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.dining-list', {
                url: '/dining/list',
                views: {
                    'content': {
                        controller: 'DiningController',
                        controllerAs: '$ctrl',
                        templateUrl: 'dining/templates/dining-list.template.html'
                    }
                }
            })
            .state('app.dining-detail', {
                url: '/dining/detail/:id',
                params: {
                    restaurant: null
                },
                views: {
                    'content': {
                        controller: 'DiningDetailController',
                        controllerAs: '$ctrl',
                        templateUrl: 'dining/templates/dining-detail.template.html'
                    }
                }
            });
    }

    angular.module('app.dining')
        .config(routingConfiguration);
}
