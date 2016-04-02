/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Core.Controllers {
    'use strict';

    import SummaryInfoAPI = Core.Services.SummaryInfoAPI;
    import SummaryInfo = Core.Models.SummaryInfo;
    import Weather = Core.Models.Weather;

    class SummaryInfoController {

        private summaryInfo: SummaryInfo;

        constructor(
            private summaryInfoAPI: SummaryInfoAPI,
            private $state
        ) {
            summaryInfoAPI.get().then((summaryInfo: SummaryInfo) => {
                this.summaryInfo = summaryInfo;
            });
        }

        public getWeather(id: number): string {
            return Weather[id];
        }
    }

    angular.module('app.core')
        .controller('SummaryInfoController', SummaryInfoController);
}
