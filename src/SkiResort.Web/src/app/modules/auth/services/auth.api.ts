/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Auth.Services {
    'use strict';

    export class AuthAPI {
        constructor(
            private $http,
            private configService: Core.Services.ConfigService,
            private $httpParamSerializerJQLike
        ) {}

        public login(username: string, password: string) {
            var request = {
                url: `${this.configService.API.URL}connect/token`,
                method: 'POST',
                data: this.$httpParamSerializerJQLike({
                    grant_type: this.configService.Authentication.GrantType,
                    client_id: this.configService.Authentication.ClientID,
                    client_secret: this.configService.Authentication.ClientSecret,
                    scope: this.configService.Authentication.Scope,
                    username: username,
                    password: password
                }),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            };
            return this.$http(request);
        }
    }

    angular.module('app.auth')
        .service('authAPI', AuthAPI);
}
