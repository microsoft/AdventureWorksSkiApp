/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Core.Services {
    'use strict';

    export class NavigationService {
        constructor(
            private $state: ng.ui.IStateService,
            private $ionicHistory
        ) {}

        public goHome() {
            this.$ionicHistory.nextViewOptions({disableBack: true, historyRoot: true});
            this.$state.go('app.home');
        }

        public goBack() {
            if (ionic.Platform['is']('browser')) {
                window.history.back();
            } else {
                this.$ionicHistory.goBack();
            }
        }
    }

    angular.module('app.core')
        .service('navigationService', NavigationService);
}
