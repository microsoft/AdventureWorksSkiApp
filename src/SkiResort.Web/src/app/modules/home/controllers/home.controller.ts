/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Home {
    'use strict';

     import AuthService = Auth.Services.AuthService;
     import AuthImageService = Auth.Services.AuthImageService;

    class HomeController {
        constructor(
            private $scope,
            private authService: AuthService,
            private authImageService: AuthImageService,
            private navigationService: Core.Services.NavigationService
        ) {}

        public getImage(id: string): any {
            return this.authImageService.get(id);
        }

        public goLogin() {
            this.navigationService.openLogin();
        }
    }

    angular.module('app.home')
        .controller('HomeController', HomeController);
}
