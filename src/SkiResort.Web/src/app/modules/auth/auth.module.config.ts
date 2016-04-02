/// <reference path='../../../../typings/browser.d.ts'/>

module SkiResort.App.Auth {
    'use strict';

    function routingConfiguration($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.login', {
                url: '/login',
                views: {
                    'content': {
                        controller: 'LoginController',
                        controllerAs: '$ctrl',
                        templateUrl: 'auth/templates/login.template.html',
                    }
                }
            });
    }

    angular.module('app.auth')
        .config(routingConfiguration);
}
