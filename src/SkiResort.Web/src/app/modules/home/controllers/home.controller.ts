/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Home {
    'use strict';

     import AuthService = Auth.Services.AuthService;
     import AuthImageService = Auth.Services.AuthImageService;

    class HomeController {
        constructor(
            private $scope,
            private authService: AuthService,
            private authImageService: AuthImageService
        ) {}

        public getImage(id: string): any {
            return this.authImageService.get(id);
        }
    }

    angular.module('app.home')
        .controller('HomeController', HomeController);
}
