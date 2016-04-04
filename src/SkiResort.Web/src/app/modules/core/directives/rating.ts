/// <reference path='../../../../../typings/browser.d.ts'/>

module SkiResort.App.Core.Directives {

    function RatingDirective() {

        var defaultClasses = 'icon ';

        function link(scope, element) {

            for (var i = 0; i < scope.value; i++) {
                element.append(angular.element(`<span class="${defaultClasses}${scope.classOn}"/>`));
            }

            for (var j = scope.value; j < scope.max; j++) {
                element.append(angular.element(`<span class="${defaultClasses}${scope.classOff}"/>`));
            }
        }

        return {
            scope: {
                value: '@',
                max: '@',
                classOn: '@',
                classOff: '@'
            },
            link: link
        };
    }

    angular.module('app.core')
        .directive('skiRating', RatingDirective);
}
