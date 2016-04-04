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
            private $ionicHistory
        ) {}

        public login() {
            this.loading = true;
            this.wrongPassword = false;
            if (this.loginForm.$valid) {
                this.authService.login(this.loginData.username, this.loginData.password)
                    .then(() => {
                        this.$ionicHistory.goBack();
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
