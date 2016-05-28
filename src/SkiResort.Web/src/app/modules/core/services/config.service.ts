/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Core.Services {
    'use strict';

    export class ConfigService {

        constructor($log: ng.ILogService) {
            $log.info('ConfigService initialized');
            if (ionic.Platform.isWebView()) {
                $log.info(' - WebView detected');
                this.API.URL = 'http://adventureworkskiresort.azurewebsites.net/';
            } else {
                $log.info('- Browser detected');
            }
        }

        public General = {
            FakeGeolocation: true,
            Geolocation: {
                latitude: 40.722846,
                longitude: -74.007325
            }
        };

        public API = {
            URL: '/',
            Path: 'api/'
        };

        public Authentication = {
            GrantType: 'password',
            ClientID: 'SkyResort',
            ClientSecret: 'secret',
            Scope: 'api'
        };
    }

    angular.module('app.core')
        .service('configService', ConfigService);
}
