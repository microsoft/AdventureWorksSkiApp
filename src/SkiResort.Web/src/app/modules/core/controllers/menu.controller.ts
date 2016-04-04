/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Core.Controllers {
    'use strict';

    class MenuController {
        constructor(
            private $ionicSideMenuDelegate: Ionic.ISideMenuDelegate,
            private $state,
            private currentUserService: Auth.Services.CurrentUserService
        ) {}

        public navigateTo(toStateName) {
            this.$state.go(toStateName);
            this.$ionicSideMenuDelegate.toggleRight(false);
        }

        public logout() {
            this.currentUserService.reset();
            this.$ionicSideMenuDelegate.toggleRight(false);
        }
    }

    angular.module('app.core')
        .controller('MenuController', MenuController);
}
