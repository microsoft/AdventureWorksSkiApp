/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Auth.Controllers {
    'use strict';

    class LoginController {

        private loginForm: ng.IFormController;
        private loginData: any = {};
        private wrongPassword: boolean = false;
        private loading: boolean = false;

        constructor(
            private authService: Services.AuthService,
            private navigationService: Core.Services.NavigationService
        ) {}

        public login() {
            this.loading = true;
            this.wrongPassword = false;
            if (this.loginForm.$valid) {
                this.authService.login(this.loginData.username, this.loginData.password)
                    .then(() => {
                        this.navigationService.closeLogin();
                    })
                    .catch(() => {
                        this.wrongPassword = true;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            }
        }
    }

    angular.module('app.auth')
        .controller('LoginController', LoginController);
}
