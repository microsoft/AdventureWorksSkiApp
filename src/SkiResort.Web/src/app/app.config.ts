/// <reference path='../../typings/browser.d.ts'/>

module SkiResort.App {
    'use strict';

    function run($ionicPlatform: Ionic.IPlatform, $log: ng.ILogService) {
        $ionicPlatform.ready(deviceReady.call(this, $log));
    }

    function deviceReady($log: ng.ILogService) {
        $log.info('Device Ready');
    }

    angular.module('app')
        .run(run);
}
