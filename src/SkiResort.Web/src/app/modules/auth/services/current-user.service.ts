/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Auth.Services {
    'use strict';

    const CURRENT_USER_STORAGE_KEY = '_currentUserData';

    export class CurrentUserService {
        public accessToken: string = null;
        public currentUser: Models.User = null;

        constructor(
            private $q,
            private $rootScope,
            private $http,
            private configService: Core.Services.ConfigService,
            private $log: ng.ILogService
        ) {
            this.load();
            if (this.accessToken) {
                this.getInfo();
            }
        }

        private save() {
            this.$log.info('CurrentUserService saving to LocalStorage');
            var currentUserCopy = angular.copy(this.currentUser);
            currentUserCopy.photo = null;
            localStorage.setItem(CURRENT_USER_STORAGE_KEY, JSON.stringify({
                accessToken: this.accessToken,
                currentUser: currentUserCopy
            }));
            this.$log.info('- Successfully saved');
        }

        private load() {
            this.$log.info('CurrentUserService loading from LocalStorage');
            var storedRawData = localStorage.getItem(CURRENT_USER_STORAGE_KEY);
            if (storedRawData) {
                var storedData = JSON.parse(storedRawData);
                this.accessToken = storedData.accessToken;
                this.currentUser = storedData.currentUser;
                this.$rootScope.currentUser = this.currentUser;
                this.$log.info('- Successfully loaded');
            } else {
                this.$log.info('- LocalStorage is empty');
            }
        }

        public reset() {
            this.$log.info('CurrentUserService resetting LocalStorage');
            this.accessToken = null;
            this.currentUser = null;
            this.$rootScope.currentUser = null;
            localStorage.setItem(CURRENT_USER_STORAGE_KEY, JSON.stringify({}));
            this.$log.info('- Done');
        }

        public getInfo() {
            this.$log.info('CurrentUserService getting information');
            var request = {
                url: `${this.configService.API.URL + this.configService.API.Path}users/user`,
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${this.accessToken}`
                }
            };

            return this.$http(request)
                .then((response) => {
                    this.$log.info('- Information getted successfully');
                    this.currentUser = response.data;
                    this.$rootScope.currentUser = this.currentUser;
                    this.save();
                })
                .catch(() => {
                    this.reset();
                    this.$log.warn('- Something went wrong while getting information');
                });
        }
    }

    angular.module('app.auth')
        .service('currentUserService', CurrentUserService);
}
