/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Auth.Services {
    'use strict';

    export class AuthService {
        constructor(
            private $q,
            private authAPI: AuthAPI,
            private currentUserService: CurrentUserService
        ) {}

        public login(username: string, password: string) {
            return this.$q((resolve, reject) => {

                this.authAPI.login(username, password)
                    .then((response) => {
                        this.currentUserService.accessToken = response.data.access_token;
                        return this.currentUserService.getInfo();
                    })
                    .then(() => {
                        resolve();
                    })
                    .catch((error) => {
                        reject(error.data);
                    });

            });
        }
    }

    angular.module('app.auth')
        .service('authService', AuthService);
}
