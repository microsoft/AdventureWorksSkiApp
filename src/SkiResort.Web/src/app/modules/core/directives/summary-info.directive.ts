module SkiResort.App.Core.Directives {

    function SummaryInfoDirective() {

        return {
            scope: {},
            restrict: 'E',
            templateUrl: 'core/templates/summary-info.template.html',
            controller: 'SummaryInfoController',
            controllerAs: '$ctrl'
        };
    }

    angular.module('app.core')
        .directive('skiSummaryInfo', SummaryInfoDirective);
}
