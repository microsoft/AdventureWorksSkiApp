/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Auth.Services {
    'use strict';

    export class AuthImageService {
        constructor(
            private $q,
            private authAPI: AuthAPI,
            private currentUserService: CurrentUserService,
            private configService: Core.Services.ConfigService
        ) { /* */ }

        public get(id: string) {
            return `${this.configService.API.URL + this.configService.API.Path}users/photo/${id}`;
        }
    }

    angular.module('app.auth')
        .service('authImageService', AuthImageService);
}
