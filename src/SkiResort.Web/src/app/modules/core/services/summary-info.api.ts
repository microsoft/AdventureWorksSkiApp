/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Core.Services {
    'use strict';

    import SummaryInfo = Core.Models.SummaryInfo;

    export class SummaryInfoAPI {

        constructor(
            private $q,
            private $http,
            private configService: Services.ConfigService
        ) {}

        public get(): angular.IHttpPromise<SummaryInfo> {
            var request = {
                url: `${this.configService.API.URL + this.configService.API.Path}summaries`,
                method: 'GET'
            };

            return this.$http(request).then(results => {
                return results.data;
            });
        }
    }

    angular.module('app.core')
        .service('summaryInfoAPI', SummaryInfoAPI);
}
